using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyCraft.Engine.Abstract;
using MyCraft.Util;
using OpenTK;

namespace MyCraft.Engine.Particles.Behaviours
{
    public class ExplosionParticleBehaviour : IParticleBehaviour
    {
        private Random _random;
        private TimeSpan _particleLifetime = TimeSpan.FromSeconds(5);
        private float particleSpeed = 30;

        private IList<Colour> Colours =new List<Colour>
        {
            new Colour { R = 0xFF, G = 0x00, B = 0x00, A = 0xFF },
            new Colour { R = 0x99, G = 0x00, B = 0x00, A = 0xFF },
            new Colour { R = 0x10, G = 0x10, B = 0x10, A = 0xFF },
            new Colour { R = 0x20, G = 0x20, B = 0x20, A = 0xFF },
            new Colour { R = 0x00, G = 0x00, B = 0x00, A = 0xFF },
        };

        public ExplosionParticleBehaviour()
        {
            _random = new Random();
        }

        public int MaxParticlesCount => 200;

        public ICollection<Particle> CreateInitialParticles()
        {
            var particles = new List<Particle>(MaxParticlesCount);
            for(int i = 0; i < MaxParticlesCount; i++)
            {
                float x = (float)(_random.NextDouble() * 5) - 2.5f;
                float z = (float)(_random.NextDouble() * 5) - 2.5f;

                var colour = Colours[_random.Next(Colours.Count)];
                var particle = new Particle(
                    new Vector3(x, 0, z),
                    colour);

                // TODO velocity for all partiles should create a sphere/cone (maybe pre compute?)
                var velocity = new Vector3(
                    (float)((_random.NextDouble() * particleSpeed) - particleSpeed / 2f),
                    (float)_random.NextDouble() * 30,
                    (float)((_random.NextDouble() * particleSpeed) - particleSpeed / 2f));
                particle.Velocity = velocity;
                particles.Add(particle);
            }
            return particles;
        }

        public Particle Spawn(FrameUpdateInfo frameInfo)
        {
            // don't spawn any more particles
            return null;
        }

        public void Update(FrameUpdateInfo frameInfo, Particle particle)
        {
            var v = particle.Velocity;
            v.Y -= Physics.Gravity / 4;
            particle.Velocity = v;
            particle.Position += particle.Velocity * (float)frameInfo.TimeDelta;
            particle.Lifetime += TimeSpan.FromSeconds(frameInfo.TimeDelta);

            if (particle.Lifetime > _particleLifetime)
            {
                particle.Destroyed = true;
            }
        }
    }
}
