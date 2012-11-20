namespace ProjectMercury.TestBench
{
    using System;
    using System.Diagnostics;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;
    using ProjectMercury.Renderers;

    public sealed class TestApp : Game
    {
        private readonly GraphicsDeviceManager GraphicsDeviceManager;

        private SpriteBatch SpriteBatch;

        private SpriteFont Font;

        private AbstractRenderer Renderer;

        private Matrix World;
        public Matrix View;
        public Matrix Projection;
        private Vector3 CameraPosition = new Vector3(0,0,950);
        private Stopwatch UpdateTimer;
        private Vector3 Up = Vector3.Up;
        private Stopwatch RenderTimer;
        private Double UpdateTime;
        private Double RenderTime;
        private ParticleEffectGameComponent[] demos;
        private int demoIndex;
        private InputComponent input;

        public TestApp()
        {
            this.GraphicsDeviceManager = new GraphicsDeviceManager(this)
            {
#if PHONE
                PreferredBackBufferWidth       = 480,
                PreferredBackBufferHeight      = 800,
#else
                PreferredBackBufferWidth = 1280   ,
                PreferredBackBufferHeight = 720,
#endif
                PreferredBackBufferFormat      = SurfaceFormat.Color,
                SynchronizeWithVerticalRetrace = true,
            };


            Content.RootDirectory = "Content";

            base.IsFixedTimeStep = false;
            this.UpdateTimer = new Stopwatch();
            this.RenderTimer = new Stopwatch();

            input = new InputComponent(this);
            Components.Add(input);

            demos = new ParticleEffectGameComponent[] {new Demo2(this), new Demo1(this)};
            demoIndex = 1;
            for (int i = 0; i < demos.Length; i++)
            {
                var demo = demos[i];
                demo.Enabled = demo.Visible = (i == demoIndex);
                Components.Add(demo);
            }
        }

        protected override void Initialize()
        {
            this.Renderer = new SpriteBatchRenderer
            {
                GraphicsDeviceService = this.GraphicsDeviceManager,
                Transformation = Matrix.CreateTranslation(GraphicsDeviceManager.GraphicsDevice.Viewport.Width / 2f, GraphicsDeviceManager.GraphicsDevice.Viewport.Height / 2f, 0f)
            }; 

            this.Renderer = new QuadRenderer(10000)
            {
                GraphicsDeviceService = this.GraphicsDeviceManager,
            };


            this.World = Matrix.Identity;
            this.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver2, this.GraphicsDeviceManager.GraphicsDevice.Viewport.AspectRatio, 1f, 5000f);

#if PHONE
            //Hide the status bar on the phone
            this.GraphicsDeviceManager.ToggleFullScreen();
#endif


            base.Initialize();
        }

        protected override void LoadContent()
        {
            this.SpriteBatch = new SpriteBatch(this.GraphicsDeviceManager.GraphicsDevice);

            this.Renderer.LoadContent(Content);

            this.Font = Content.Load<SpriteFont>("Segoe");

            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            Counters.StartFrame();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                base.Exit();

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                base.Exit();

            if (input.IsKeyPressed(Keys.P) || input.IsButtonPressed(Buttons.Start))
            {
                demos[demoIndex].Enabled = !demos[demoIndex].Enabled;
            }

            if (input.IsButtonPressed(Buttons.DPadRight) ||input.IsKeyPressed(Keys.PageDown))
            {
                demos[demoIndex].Enabled = false;
                demoIndex = (demoIndex + 1)%demos.Length;
                demos[demoIndex].Enabled = true;
            }

            if (input.IsButtonPressed(Buttons.DPadLeft) || input.IsKeyPressed(Keys.PageUp))
            {
                demos[demoIndex].Enabled = false;
                demoIndex = (demoIndex - 1 + demos.Length) % demos.Length;
                demos[demoIndex].Enabled = true;
            }

            this.UpdateTimer.Start();
            base.Update(gameTime);
            this.UpdateTimer.Stop();
            this.UpdateTime = this.UpdateTimer.Elapsed.TotalSeconds;
            this.UpdateTimer.Reset();

            //very basic orbit/zoom camera
            GamePadState state = GamePad.GetState(PlayerIndex.One);
            var cameraTransform = Matrix.Identity;
            cameraTransform *= Matrix.CreateScale((float)Math.Pow(1.01, state.Triggers.Left - state.Triggers.Right + ((input.KeyboardState.IsKeyDown(Keys.Home)) ? 1 : 0) - ((input.KeyboardState.IsKeyDown(Keys.End)) ? 1 : 0)));
            cameraTransform *= Matrix.CreateFromAxisAngle(Vector3.Normalize(Up), (state.ThumbSticks.Right.X+((input.KeyboardState.IsKeyDown(Keys.Right)) ? 1 : 0) - ((input.KeyboardState.IsKeyDown(Keys.Left)) ? 1 : 0))/10f);
            var currentLeft = Vector3.Cross(CameraPosition, Up);
            cameraTransform *= Matrix.CreateFromAxisAngle(Vector3.Normalize(currentLeft), (state.ThumbSticks.Right.Y +((input.KeyboardState.IsKeyDown(Keys.Up)) ? 1 : 0) - ((input.KeyboardState.IsKeyDown(Keys.Down)) ? 1 : 0))/ 10f );
            CameraPosition = Vector3.Transform(CameraPosition, cameraTransform);
            Up = Vector3.Transform(Up, cameraTransform);
            this.View = Matrix.CreateLookAt(CameraPosition, new Vector3(0f, 0f, 0f), Up);
        }

        protected override void Draw(GameTime gameTime)
        {
            this.GraphicsDeviceManager.GraphicsDevice.Clear(Color.Black);
            this.RenderTimer.Start();
            this.Renderer.RenderEffect(demos[demoIndex].ParticleEffect, ref this.World, ref this.View, ref this.Projection, ref this.CameraPosition);
            this.RenderTimer.Stop();
            this.RenderTime = this.RenderTimer.Elapsed.TotalSeconds;
            this.RenderTimer.Reset();
            this.SpriteBatch.Begin();
            this.SpriteBatch.DrawString(this.Font, string.Format("{0:#.##} FPS", 1f / gameTime.ElapsedGameTime.TotalSeconds), new Vector2(40f, 20f), Color.White);
            this.SpriteBatch.DrawString(this.Font, string.Format("Particles Drawn {0} Updated {1} Triggered {2} Trigger Culled {3}", Counters.ParticlesDrawn, Counters.ParticlesUpdated, Counters.ParticlesTriggered, Counters.ParticleTriggersCulled), new Vector2(40f, 40f), Color.White);
            this.SpriteBatch.DrawString(this.Font, string.Format("{0} {1:##.###}", "Update Time:", this.UpdateTime), new Vector2(40f, 60f), Color.White);
            this.SpriteBatch.DrawString(this.Font, string.Format("{0} {1:##.###}", "Render Time:", this.RenderTime), new Vector2(240, 60f), Color.White);

            this.SpriteBatch.DrawString(this.Font, demos[demoIndex].Description, new Vector2(40, GraphicsDevice.Viewport.Height - Font.MeasureString(demos[demoIndex].Description).Y), Color.GreenYellow);
            this.SpriteBatch.End();


        }
    }
}