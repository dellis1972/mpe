/*
 * Copyright © 2010 Project Mercury Team Members (http://mpe.codeplex.com/People/ProjectPeople.aspx)
 * 
 * This program is licensed under the Microsoft Permissive License (Ms-PL). You should
 * have received a copy of the license along with the source code. If not, an online copy
 * of the license can be found at http://mpe.codeplex.com/license.
 */

namespace $rootnamespace$
{
    using Microsoft.Xna.Framework;
    using ProjectMercury;
    using ProjectMercury.Modifiers;

    /// <summary>
    /// 
    /// </summary>
    public sealed class $safeitemname$ : AbstractModifier
    {
        /// <summary>
        /// Creates a deep copy of this instance.
        /// </summary>
        /// <returns>A new instance of $safeitemname$ which is a copy of this instance.</returns>
        public override AbstractModifier DeepCopy()
        {
            return new $safeitemname$
            {
                // TODO clone $safeitemname$ fields and properties...
            };
        }

        /// <summary>
        /// Processes active particles.
        /// </summary>
        /// <param name="deltaSeconds">Elapsed time in whole and fractional seconds.</param>
        /// <param name="particle">A pointer to the first element in an array of particles.</param>
        /// <param name="count">The number of particles which need to be processed.</param>
        protected internal override unsafe void Process(float deltaSeconds, Particle* particle, int count)
        {
            for (int i = 0; i < count; i++)
            {
                // TODO modifier logic...

                particle++;
            }
        }
    }
}