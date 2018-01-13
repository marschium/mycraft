using MyCraft.Engine.Abstract;
using MyCraft.Engine.Core.Blocks;
using MyCraft.Engine.GameObjects;
using MyCraft.Engine.GameObjects.Camera;
using MyCraft.Engine.GameObjects.Components;
using MyCraft.Engine.Particles.Behaviours;
using MyCraft.Engine.World;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCraft.Engine.Core.GameObjects
{
    public class Explosion : GameObject
    {
        private static int MaxFireSpwans = 5;

        private ParticleComponent _particleComponent;
        private bool _scheduledStop = false;
        private int _radius = 3;

        private Random _random;
        private double _fireSpawnChance = 0.05f;
        private int _spawnedFires;
        private bool _destroyedBlocks;

        public Explosion(BlockMap blockMap, GameObjectInitaliser gameObjectInitaliser) : base(blockMap, gameObjectInitaliser)
        {
            _random = new Random();
        }

        protected override void OnLoad()
        {
            _particleComponent = new ParticleComponent(this, new Vector3(0.5f, 1, 0.5f), new ExplosionParticleBehaviour())
            {
                Scale = 0.3f,
            };
            Components.Add(_particleComponent);
            base.OnLoad();
        }

        protected override void OnUpdate(ICamera camera, FrameUpdateInfo frameInfo)
        {
            if (!_scheduledStop)
            {
                UpdateScheduler.Schedule(TimeSpan.FromSeconds(5), () => Destroy());
                _scheduledStop = true;
            }

            if (!_destroyedBlocks)
            {
                IEnumerable<(Vector3, Block)> blocksToDestory = new List<(Vector3, Block)>();
                for (int y = 0; y <= _radius; y++)
                {
                    var r = _radius - y;
                    blocksToDestory = blocksToDestory.Concat(DestoryCircle(r, y));
                    if (y != 0)
                    {
                        blocksToDestory = blocksToDestory.Concat(DestoryCircle(r, -y));
                    }
                }
                BlockMap.SetBlocks(blocksToDestory);
                _destroyedBlocks = true;
            }
            

            base.OnUpdate(camera, frameInfo);
        }

        /// <summary>
        /// Draw a circle parralel to the y axis
        /// </summary>
        private IEnumerable<(Vector3, Block)> DestoryCircle(int r, int dy)
        {
            int error = -r;
            int x = r;
            int z = 0;

            IEnumerable<(Vector3, Block)> blocksToDestory = new List<(Vector3, Block)>();
            while (x >= z)
            {
                int lastZ = z;

                error += z;
                ++z;
                error += z;

                blocksToDestory = blocksToDestory.Concat(DestoryHorizontalLine(x, lastZ, dy));

                if (error >= 0)
                {
                    if (x != lastZ)
                        blocksToDestory = blocksToDestory.Concat(DestoryHorizontalLine(lastZ, x, dy));

                    error -= x;
                    --x;
                    error -= x;
                }
            }
            return blocksToDestory;
        }

        private IEnumerable<(Vector3, Block)> DestoryHorizontalLine(int dx, int dz, int dy)
        {
            //void DestroyBlock(Vector3 pos)
            //{
            //    // TODO save and run after blocks have been destroyed
            //    //if (BlockMap.BlockMatches(pos, b => b?.GetType() == typeof(ExplosiveBlock)))
            //    //{
            //    //    // TODO maybe use behaviour class?
            //    //    GameObjectInitaliser.Create<Explosion>(pos);
            //    //}
            //    BlockMap.SetBlock(pos, null);
            //}

            void CreateFire(Vector3 pos)
            {
                if (_random.NextDouble() < _fireSpawnChance && _spawnedFires < MaxFireSpwans)
                {
                    GameObjectInitaliser.Create<Fire>(pos);
                    ++_spawnedFires;
                }
            }

            var blocksToDestory = new List<(Vector3, Block)>(dx * 2);
            for(int x = -dx; x < dx; x++)
            {
                var pos = Position + new Vector3(x, dy, dz);
                blocksToDestory.Add((pos, null));
                CreateFire(pos - Vector3.UnitY); // TODO do after removing blocks

                pos = Position + new Vector3(x, dy, -dz);
                blocksToDestory.Add((pos, null));
                CreateFire(Position + new Vector3(x, dy - 1, -dz)); // TODO do after removing blocks
            }
            return blocksToDestory;
        }
    }
}
