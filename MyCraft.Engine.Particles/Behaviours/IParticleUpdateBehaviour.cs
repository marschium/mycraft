using MyCraft.Engine.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCraft.Engine.Particles.Behaviours
{
    public interface IParticleUpdateBehaviour
    {
        void Update(FrameUpdateInfo frameInfo, Particle particle);
    }
}
