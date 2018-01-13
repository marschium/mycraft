using MyCraft.Engine.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCraft.Engine.Particles.Behaviours
{
    /// <summary>
    /// Interface for controlling how/when particles are created.
    /// </summary>
    public interface IParticleSpawnBehaviour
    {
        int MaxParticlesCount { get; }

        /// <summary>
        /// Run the behaviour once. If a new particle has been created, it is returned. Otherwise Null is returned.
        /// </summary>
        /// <param name="frameInfo"></param>
        /// <returns></returns>
        Particle Spawn(FrameUpdateInfo frameInfo);
    }
}
