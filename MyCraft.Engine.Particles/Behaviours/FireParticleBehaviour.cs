using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyCraft.Engine.Abstract;
using OpenTK;

namespace MyCraft.Engine.Particles.Behaviours
{
    public class FireParticleBehaviour : IParticleBehaviour
    {
        private Random _random = new Random();
        private TimeSpan _timeSinceParticleGen;

        public TimeSpan ParticleLifeTime => TimeSpan.FromSeconds(10);

        public TimeSpan TimeBetweenParticles => TimeSpan.FromMilliseconds(300);

        public int MaxParticlesCount => 10;

        private int _framesSinceColourChange;

        public Particle Spawn(FrameUpdateInfo frameInfo)
        {
            if (_timeSinceParticleGen > TimeBetweenParticles)
            {
                float x = (float) (_random.NextDouble() * 5) - 2.5f; // TODO use particle size to determine correct offset from center of system
                float z = (float) (_random.NextDouble() * 5) - 2.5f;

                var particle = new Particle(
                    new Vector3(x, 0, z),
                    new Colour { R = 0x00, G = 0x00, B = 0x00, A = 0xFF });
                particle.Velocity = new Vector3(0, 1, 0).Normalized();
                _timeSinceParticleGen = TimeSpan.Zero;
                return particle;
            }
            else
            {
                _timeSinceParticleGen += TimeSpan.FromSeconds(frameInfo.TimeDelta);
            }
            return null;
        }

        public void Update(FrameUpdateInfo frameUpdateInfo, Particle particle)
        {
            particle.Position += particle.Velocity * (float)frameUpdateInfo.TimeDelta;
            particle.Lifetime += TimeSpan.FromSeconds(frameUpdateInfo.TimeDelta);

            if (particle.Colour.R < 255 && _framesSinceColourChange >= 3)
            {
                particle.Colour.R += 1;
                particle.Colour.G += 1;
                particle.Colour.B += 1;
                particle.Colour.A -= 1;
                _framesSinceColourChange = 0;
            }
            else
            {
                _framesSinceColourChange++;
            }

            if (particle.Lifetime > ParticleLifeTime)
            {
                particle.Destroyed = true;
            }
        }

        public ICollection<Particle> CreateInitialParticles()
        {
            return new List<Particle>();
        }
    }
}
