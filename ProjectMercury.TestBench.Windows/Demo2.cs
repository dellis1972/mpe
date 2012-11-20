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
    class Demo2 : ParticleEffectGameComponent
    {

        private PointToPointProxy Proxy1;
        private ParticleEffectProxy Proxy2;

        public Demo2(TestApp game)
            : base(game)
        {
        }

        public override void Initialize()
        {
            this.ParticleEffect = new ParticleEffect
                                      {
                                          Emitters = new EmitterCollection
                                                         {
                                                             new LineEmitter
                                                                 {
                                                                     Budget = 15000,
                                                                     BillboardStyle = BillboardStyle.Spherical,
                                                                     Length = 800,
                                                                     EmitBothWays = true,
                                                                     ConstrainToPlane = true,
                                                                     Rectilinear = true,
                                                                     Term = 3f,
                                                                     BlendMode = EmitterBlendMode.Add,
                                                                     ReleaseColour = Color.SteelBlue.ToVector3(),
                                                                     ReleaseOpacity = 1,
                                                                     ReleaseQuantity = 20,
                                                                     ReleaseScale = new Range(16f, 22f),
                                                                     ReleaseSpeed = new Range(1f, 3f),
                                                                     Modifiers = new ModifierCollection
                                                                                     {
                                                                                         new OpacityFastFadeModifier
                                                                                             {InitialOpacity = 0.25f},
                                                                                         new ColourInterpolator2
                                                                                             {
                                                                                                 InitialColour =
                                                                                                     Color.DarkRed.
                                                                                                     ToVector3(),
                                                                                                 FinalColour =
                                                                                                     Color.Yellow.
                                                                                                     ToVector3()
                                                                                             },
                  },
                                                                 },
                                                         },
                                      };




            this.ParticleEffect.Emitters[0].Initialise();

            Proxy1 = new PointToPointProxy(ParticleEffect) { Start = new Vector3(0,0,0), End = new Vector3(121, -210, -310), OriginalEffectSize  = 800};
            Proxy2 = new ParticleEffectProxy(ParticleEffect);
            
            base.Initialize();
        }


        protected override void LoadContent()
        {
            this.ParticleEffect.Emitters[0].ParticleTexture = Game.Content.Load<Texture2D>("Star");
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            Vector3 triggerPosition = Vector3.Zero;
            this.ParticleEffect.Trigger(ref triggerPosition);
            Proxy1.Trigger();
            Proxy2.Trigger();
            this.ParticleEffect.Update((float)gameTime.ElapsedGameTime.TotalSeconds);

            //Rotate the proxy in an ellipse around the original
            var pos = new Vector3(233f * (float)Math.Sin(gameTime.TotalGameTime.TotalSeconds / 1.3), 143f * (float)Math.Sin(gameTime.TotalGameTime.TotalSeconds), 517f * (float)Math.Cos(gameTime.TotalGameTime.TotalSeconds * 1.7));

            Proxy2.World = Matrix.CreateTranslation(pos);
            Proxy1.End = pos;


            base.Update(gameTime);
        }

        public override string Description
        {
            get { return "Demo2:\nPointToPoint proxy showing an effect proxy that tracks betweeen 2 points (centers of the other lines)"; }
        }
    }
}
