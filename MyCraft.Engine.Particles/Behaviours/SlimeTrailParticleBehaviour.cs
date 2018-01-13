using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyCraft.Engine.Abstract;
using OpenTK;

namespace MyCraft.Engine.Particles.Behaviours
{
    public class SlimeTrailParticleBehaviour : IParticleBehaviour
    {
        private Random random = new Random();
        public int StartingParticles => 3;

        private int _framesSinceColourChange;

        public int MaxParticlesCount => 5;

        public ICollection<Particle> CreateInitialParticles()
        {
            var particles = new List<Particle>();
            for(int i = 0; i < StartingParticles; i++)
            {
                var x = (float) ((random.NextDouble() * 2) - 1f);
                var z = (float) ((random.NextDouble() * 2) - 1f);
                var particle = new Particle(new Vector3(x, 0, z), new Colour { R = 0xF, A = 0xFF, B = 0xF, G = 0xFF });
                particles.Add(particle);
            }
            return particles;
        }

        public Particle Spawn(FrameUpdateInfo frameInfo)
        {
            return null; // do not create any new particles
        }

        public void Update(FrameUpdateInfo frameInfo, Particle particle)
        {
            if (particle.Colour.A > 0 && _framesSinceColourChange >= 3)
            {
                particle.Colour.A -= 1;
                _framesSinceColourChange = 0;
            }
            else if (particle.Colour.A == 0)
            {
                particle.Destroyed = true;
            }
            else
            {
                _framesSinceColourChange++;
            }
        }
    }
}
