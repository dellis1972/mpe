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
    using ProjectMercury.Emitters;

    /// <summary>
    /// 
    /// </summary>
    public sealed class $safeitemname$ : AbstractEmitter
    {
        /// <summary>
        /// Copies the properties of this instance into the specified existing instance.
        /// </summary>
        /// <param name="exisitingInstance">An existing emitter instance.</param>
        protected override AbstractEmitter DeepCopy(AbstractEmitter exisitingInstance)
        {
            $safeitemname$ value = (exisitingInstance as $safeitemname$) ?? default($safeitemname$);

            // TODO copy implementation specific fields & properties...

            base.DeepCopy(value);

            return value;
        }

        /// <summary>
        /// Generates offset and force vectors for a newly released particle.
        /// </summary>
        /// <param name="offset">Defines an offset vector from the trigger position.</param>
        /// <param name="force">A unit vector defining the inital force applied to the particle.</param>
        protected override void GenerateOffsetAndForce(out Vector3 offset, out Vector3 force)
        {
            // TODO generate offset and force vectors...
            base.GenerateOffsetAndForce(out offset, out force);
        }
    }
}