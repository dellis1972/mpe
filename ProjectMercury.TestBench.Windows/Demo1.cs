using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjectMercury.Controllers;
using ProjectMercury.Emitters;
using ProjectMercury.Modifiers;
using ProjectMercury.Proxies;

namespace ProjectMercury.TestBench
{
    class Demo1 : ParticleEffectGameComponent
    {
        private ParticleEffectProxy Proxy1;
        private ParticleEffectProxy Proxy2;

        public Demo1(TestApp game)
            : base(game)
        {
        }

        public override void Initialize()
        {
            this.LoadContent();

            for (int i = 0; i < this.ParticleEffect.Emitters.Count; i++)
                this.ParticleEffect.Emitters[i].Initialise();
        }

        protected override void LoadContent()
        {
            this.ParticleEffect = Game.Content.Load<ParticleEffect>("Demo1");

            for (int i = 0; i < this.ParticleEffect.Emitters.Count; i++)
                this.ParticleEffect.Emitters[i].ParticleTexture = Game.Content.Load<Texture2D>("Star");

            Proxy1 = new ParticleEffectProxy(ParticleEffect) { World = Matrix.CreateScale(.5f) * Matrix.CreateTranslation(800, 0, 0) };
            Proxy2 = new ParticleEffectProxy(ParticleEffect) { World = Matrix.CreateScale(.1f) * Matrix.CreateTranslation(-900, 0, 0) };
            
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            (this.ParticleEffect.Emitters[0].Controllers[0] as TriggerOffsetController).TriggerOffset = new Vector3
            {
                X = (float)Math.Cos(gameTime.TotalGameTime.TotalSeconds * 1.15f) * 275f,
                Y = (float)Math.Sin(gameTime.TotalGameTime.TotalSeconds * 1.15f) * 250f,
                Z = (float)Math.Cos(gameTime.TotalGameTime.TotalSeconds * 0.75) * 290f,
            };

            (this.ParticleEffect.Emitters[1].Controllers[0] as TriggerOffsetController).TriggerOffset = new Vector3
            {
                X = (float)Math.Cos(gameTime.TotalGameTime.TotalSeconds * 1f) * -275f,
                Y = (float)Math.Sin(gameTime.TotalGameTime.TotalSeconds * 1.15f) * -250f,
                Z = (float)Math.Sin(gameTime.TotalGameTime.TotalSeconds * 0.75) * -290f,
            };

            (this.ParticleEffect.Emitters[2].Controllers[0] as TriggerOffsetController).TriggerOffset = new Vector3
            {
                X = (float)Math.Sin(gameTime.TotalGameTime.TotalSeconds * 0.75f) * 275f,
                Y = (float)Math.Cos(gameTime.TotalGameTime.TotalSeconds * 1f) * -250f,
                Z = (float)Math.Sin(gameTime.TotalGameTime.TotalSeconds * 1.15f) * -290f,
            };

            Vector3 triggerPosition = Vector3.Zero;

            var frustum = new BoundingFrustum(((TestApp)Game).View * ((TestApp)Game).Projection);
            ParticleEffect.Trigger(ref triggerPosition, ref frustum);
            Proxy1.Trigger(ref frustum);
            Proxy2.Trigger(ref frustum);
            ParticleEffect.Update((float)gameTime.ElapsedGameTime.TotalSeconds);

            Matrix rotateInstances1;
            Matrix.CreateRotationY(.01f, out rotateInstances1);
            Matrix.Multiply(ref Proxy2.World, ref rotateInstances1, out Proxy2.World);

            Matrix rotateInstance2;
            Matrix.CreateRotationX(.1f, out rotateInstance2);
            Matrix.Multiply(ref Proxy1.World, ref rotateInstance2, out Proxy1.World);

            base.Update(gameTime);
        }

        public override string Description
        {
            get { return "Demo1:\n3 proxy effects each with a different transformation\nred = Billboard.None, green=.Spherical, blue=.Cylindrical"; }
        }
    }
}
