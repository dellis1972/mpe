using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ProjectMercury.TestBench
{
    abstract class ParticleEffectGameComponent : DrawableGameComponent
    {
        protected ParticleEffectGameComponent(TestApp game)
            : base(game)
        {
        }

        public virtual ParticleEffect ParticleEffect { get; protected set; }

        public abstract string Description { get; }
    }
}
