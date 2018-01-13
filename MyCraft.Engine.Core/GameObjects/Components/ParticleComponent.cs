using MyCraft.Engine.Abstract;
using MyCraft.Engine.Particles;
using MyCraft.Engine.Particles.Behaviours;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCraft.Engine.GameObjects.Components
{
    public class ParticleComponent : AbstractComponent
    {
        public ParticleComponent(GameObject parent, Vector3 offset, IParticleBehaviour particleBehaviour) : base(parent, offset)
        {
            ParticleSystem = new ParticleSystem(particleBehaviour);
        }

        public ParticleSystem ParticleSystem { get; }

        public override void Update(FrameUpdateInfo frameInfo)
        {
            var finished = ParticleSystem.Update(frameInfo);
            if (finished)
            {
                Destroyed = true; // TODO call destroy method
            }
        }
    }
}
