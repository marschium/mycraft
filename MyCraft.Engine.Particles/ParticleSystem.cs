using MyCraft.Engine.Abstract;
using MyCraft.Engine.Particles.Behaviours;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCraft.Engine.Particles
{
    public class ParticleSystem
    {
        private Vector3[] _positions;
        private uint[] _colours;
        private bool _stopped;
        private bool _ranSetup;

        private TimeSpan TimeBetweenParticles = TimeSpan.FromMilliseconds(100);
        private TimeSpan _timeSinceParticleGen = TimeSpan.Zero;

        private Random _random;

        // TODO : add support for systems that are parallel to the floor
        public static readonly Vector3[] BillboardVertices = new Vector3[]
        {
            new Vector3(-0.5f, -0.5f, -0.5f),
            new Vector3(-0.5f, 0.5f, -0.5f),
            new Vector3(0.5f, 0.5f, -0.5f),
            new Vector3(-0.5f, -0.5f, -0.5f),
            new Vector3(0.5f, 0.5f, -0.5f),
            new Vector3(0.5f, -0.5f, -0.5f),
        };

        public static readonly Vector3[] FloorVertices = new Vector3[]
        {
            new Vector3(-0.5f, 0f, -0.5f),
            new Vector3(0.5f, 0f, 0.5f),
            new Vector3(-0.5f, 0f, 0.5f),
            new Vector3(-0.5f, 0f, -0.5f),
            new Vector3(0.5f, 0f, -0.5f),
            new Vector3(0.5f, 0f, 0.5f),
        };

        public ParticleSystem(IParticleBehaviour behaviour) : this(behaviour, behaviour, behaviour)
        {
        }

        public ParticleSystem(IParticleSpawnBehaviour spawner, IParticleUpdateBehaviour behaviour, IParticleSetupBehaviour setup)
        {
            MaxParticles = spawner.MaxParticlesCount;
            _random = new Random();
            ParticleBehaviour = behaviour;
            ParticleSpawner = spawner;
            ParticleSetup = setup;
            Particles = new List<Particle>
            {
            };
            _positions = new Vector3[MaxParticles];
            _colours = new uint[MaxParticles];
            
        }

        public IList<Particle> Particles { get; }

        public int MaxParticles { get; }

        public Vector3[] ParticlePositions {
            get
            {
                return _positions;
            }
        }

        public uint[] ParticleColours
        {
            get
            {
                return _colours;
            }
        }

        public IParticleSpawnBehaviour ParticleSpawner { get; }

        public IParticleUpdateBehaviour ParticleBehaviour { get; }

        public IParticleSetupBehaviour ParticleSetup { get; }

        public int VertexBufferId { get; private set; }

        public int PositionBufferId { get; private set; }

        public int ColourBufferId { get; private set; }

        public bool Loaded { get; private set; }

        public bool Finished { get; private set; }

        public ParticleOrientation ParticleOrientation { get; set; } = ParticleOrientation.Billboard;

        public Vector3[] Vertices
        {
            get
            {
                if (ParticleOrientation == ParticleOrientation.Billboard)
                {
                    return BillboardVertices;
                }
                else if (ParticleOrientation == ParticleOrientation.Floor)
                {
                    return FloorVertices;
                }
                return BillboardVertices;
            }
            private set { }
        }

        public void Stop()
        {
            _stopped = true;
        }

        public void BuildBuffers()
        {
            // shared buffer for particle shape
            // buffer for particle positions
            // buffer for particle colours
            
            VertexBufferId = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferId);
            GL.BufferData(BufferTarget.ArrayBuffer, Vertices.Length * 3 * sizeof(float), Vertices, BufferUsageHint.StaticDraw);
            
            PositionBufferId = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, PositionBufferId);
            GL.BufferData(BufferTarget.ArrayBuffer, MaxParticles * 3 * sizeof(float), IntPtr.Zero, BufferUsageHint.StreamDraw);

            ColourBufferId = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, ColourBufferId);
            GL.BufferData(BufferTarget.ArrayBuffer, MaxParticles * 4 * sizeof(float), IntPtr.Zero, BufferUsageHint.StreamDraw);

            Loaded = true;
        }

        public bool Update(FrameUpdateInfo frameInfo)
        {
            if (!_ranSetup)
            {
                foreach(var p in ParticleSetup?.CreateInitialParticles())
                {
                    Particles.Add(p);
                }
                _ranSetup = true;
            }

            var idx = 0;
            var particlesToRemove = new List<int>();
            foreach(var particle in Particles)
            {
                ParticleBehaviour.Update(frameInfo, particle);
                if (particle.Destroyed)
                {
                    particlesToRemove.Add(idx);
                }
                else
                {
                    _positions[idx] = particle.Position;
                    _colours[idx] = particle.Colour.AsUint();
                    idx++;
                }
            }
            particlesToRemove.ForEach(i => Particles.RemoveAt(i));

            // if there is room for more particles create some
            if (Particles.Count < MaxParticles && !_stopped)
            {
                var particle = ParticleSpawner.Spawn(frameInfo);
                if (particle != null)
                {
                    Particles.Add(particle);
                }
            }

            // if we have been stopped and there are no more particles signal that we are finished
            if (_stopped && Particles.Count == 0)
            {
                Finished = true;
                return true;
            }
            else
            {
                Finished = false;
                return false;
            }
        }
    }
}
