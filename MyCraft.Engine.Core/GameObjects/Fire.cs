using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyCraft.Engine.GameObjects.Camera;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using MyCraft.Assets;
using MyCraft.Engine.Abstract;
using MyCraft.Engine.World;

using MyCraft.Engine.Particles;
using MyCraft.Engine.Particles.Behaviours;
using MyCraft.Engine.GameObjects.Components;
using MyCraft.Engine.Core.GameObjects.Components;
using MyCraft.Engine.Core.Blocks;
using MyCraft.Engine.Core.GameObjects;

namespace MyCraft.Engine.GameObjects
{
    public class Fire : GameObject
    {
        public const float MaxLifetime = 10f;

        public static Vector3[] Vertices = new Vector3[]
          {
            // back face
            new Vector3(-0.1f, -0.1f, -0.1f),
            new Vector3(-0.1f, 1.1f, -0.1f),
            new Vector3(1.1f, 1.1f, -0.1f),
            new Vector3(-0.1f, -0.1f, -0.1f),
            new Vector3(1.1f, 1.1f, -0.1f),
            new Vector3(1.1f, -0.1f, -0.1f),
            // right face
            new Vector3(1.1f, -0.1f, -0.1f),
            new Vector3(1.1f, 1.1f, -0.1f),
            new Vector3(1.1f, 1.1f, 1.1f),
            new Vector3(1.1f, -0.1f, -0.1f),
            new Vector3(1.1f, 1.1f, 1.1f),
            new Vector3(1.1f, -0.1f, 1.1f),
            // left face
            new Vector3(-0.1f, -0.1f, -0.1f),
            new Vector3(-0.1f, -0.1f, 1.1f),
            new Vector3(-0.1f, 1.1f, 1.1f),
            new Vector3(-0.1f, -0.1f, -0.1f),
            new Vector3(-0.1f, 1.1f, 1.1f),
            new Vector3(-0.1f, 1.1f, -0.1f),
            // front face
            new Vector3(-0.1f, -0.1f, 1.1f),
            new Vector3(1.1f, -0.1f, 1.1f),
            new Vector3(1.1f, 1.1f, 1.1f),
            new Vector3(-0.1f, -0.1f, 1.1f),
            new Vector3(1.1f, 1.1f, 1.1f),
            new Vector3(-0.1f, 1.1f, 1.1f),
            // front face reverse side
            new Vector3(-0.1f, -0.1f, 1.1f),
            new Vector3(-0.1f, 1.1f, 1.1f),
            new Vector3(1.1f, 1.1f, 1.1f),
            new Vector3(-0.1f, -0.1f, 1.1f),
            new Vector3(1.1f, 1.1f, 1.1f),
            new Vector3(1.1f, -0.1f, 1.1f),
            // back face revserse side
            new Vector3(-0.1f, -0.1f, -0.1f),
            new Vector3(1.1f, -0.1f, -0.1f),
            new Vector3(1.1f, 1.1f, -0.1f),
            new Vector3(-0.1f, -0.1f, -0.1f),
            new Vector3(1.1f, 1.1f, -0.1f),
            new Vector3(-0.1f, 1.1f, -0.1f),
            // right face reverse
            new Vector3(1.1f, -0.1f, -0.1f),
            new Vector3(1.1f, -0.1f, 1.1f),
            new Vector3(1.1f, 1.1f, 1.1f),
            new Vector3(1.1f, -0.1f, -0.1f),
            new Vector3(1.1f, 1.1f, 1.1f),
            new Vector3(1.1f, 1.1f, -0.1f),
            // left face reverse
            new Vector3(-0.1f, -0.1f, -0.1f),
            new Vector3(-0.1f, 1.1f, -0.1f),
            new Vector3(-0.1f, 1.1f, 1.1f),
            new Vector3(-0.1f, -0.1f, -0.1f),
            new Vector3(-0.1f, 1.1f, 1.1f),
            new Vector3(-0.1f, -0.1f, 1.1f),

            // crossed faces
            new Vector3(-0.1f,-0.1f,-0.1f),
            new Vector3(-0.1f,1.1f,-0.1f),
            new Vector3(1.1f,1.1f,1.1f),
            new Vector3(-0.1f,-0.1f,-0.1f),
            new Vector3(1.1f,1.1f,1.1f),
            new Vector3(1.1f,-0.1f,1.1f),

            new Vector3(-0.1f,-0.1f,-0.1f),
            new Vector3(1.1f,-0.1f,1.1f),
            new Vector3(1.1f,1.1f,1.1f),
            new Vector3(-0.1f,-0.1f,-0.1f),
            new Vector3(1.1f,1.1f,1.1f),
            new Vector3(-0.1f,1.1f,-0.1f),

            new Vector3(1.1f,-0.1f,-0.1f),
            new Vector3(1.1f,1.1f,-0.1f),
            new Vector3(-0.1f,1.1f,1.1f),
            new Vector3(1.1f,-0.1f,-0.1f),
            new Vector3(-0.1f,1.1f,1.1f),
            new Vector3(-0.1f,-0.1f,1.1f),

            new Vector3(1.1f,-0.1f,-0.1f),
            new Vector3(-0.1f,-0.1f,1.1f),
            new Vector3(-0.1f,1.1f,1.1f),
            new Vector3(1.1f,-0.1f,-0.1f),
            new Vector3(-0.1f,1.1f,1.1f),
            new Vector3(1.1f,1.1f,-0.1f),
    };

        public static Vector2[] TextureCoords = new Vector2[]
        {
            // back
            // top tri
            new Vector2(1f,1f),
            new Vector2(1f,0f),
            new Vector2(0f,0f),
            // bottom tri
            new Vector2(1f,1f),
            new Vector2(0f,0f),
            new Vector2(0f,1f),

            // right
            // top tri
            new Vector2(1f,1f),
            new Vector2(1f,0f),
            new Vector2(0f,0f),
            // bottom tri
            new Vector2(1f,1f),
            new Vector2(0f,0f),
            new Vector2(0f,1f),

            //left
            // top tri
            new Vector2(1f,1f),
            new Vector2(0f,1f),
            new Vector2(0f,0f),

            // bottom tri
            new Vector2(1f,1f),
            new Vector2(0f,0f),
            new Vector2(1f,0f),

            //front
            new Vector2(1f,1f),
            new Vector2(0f,1f),
            new Vector2(0f,0f),
            // bottom tri
            new Vector2(1f,1f),
            new Vector2(0f,0f),
            new Vector2(1f,0f),

            // front reverse
            // top tri
            new Vector2(1f,1f),
            new Vector2(1f,0f),
            new Vector2(0f,0f),
            // bottom tri
            new Vector2(1f,1f),
            new Vector2(0f,0f),
            new Vector2(0f,1f),

            //back reverse
            new Vector2(1f,1f),
            new Vector2(0f,1f),
            new Vector2(0f,0f),
            // bottom tri
            new Vector2(1f,1f),
            new Vector2(0f,0f),
            new Vector2(1f,0f),
            
            // right reverse
            // top tri
            new Vector2(1f,1f),
            new Vector2(0f,1f),
            new Vector2(0f,0f),

            // bottom tri
            new Vector2(1f,1f),
            new Vector2(0f,0f),
            new Vector2(1f,0f),
            
            // left reverse
            // top tri
            new Vector2(1f,1f),
            new Vector2(1f,0f),
            new Vector2(0f,0f),
            // bottom tri
            new Vector2(1f,1f),
            new Vector2(0f,0f),
            new Vector2(0f,1f),

            // crossed faces
            // top tri
            new Vector2(1f,1f),
            new Vector2(1f,0f),
            new Vector2(0f,0f),
            // bottom tri
            new Vector2(1f,1f),
            new Vector2(0f,0f),
            new Vector2(0f,1f),

            // top tri
            new Vector2(1f,1f),
            new Vector2(0f,1f),
            new Vector2(0f,0f),

            // bottom tri
            new Vector2(1f,1f),
            new Vector2(0f,0f),
            new Vector2(1f,0f),

            //top tri
            new Vector2(1f,1f),
            new Vector2(1f,0f),
            new Vector2(0f,0f),

            // bottom tri
            new Vector2(1f,1f),
            new Vector2(0f,0f),
            new Vector2(0f,1f),

            // top tri
            new Vector2(1f,1f),
            new Vector2(0f,1f),
            new Vector2(0f,0f),

            // bottom tri
            new Vector2(1f,1f),
            new Vector2(0f,0f),
            new Vector2(1f,0f),
        };

        private FireDamageBehaviour _fireDamageBehaviour;
        private TextureLoader _textureLoader;
        private ParticleComponent _particleComponent;
        private ModelComponent _modelComponent;
        private bool _fireStopped;

        public Fire(BlockMap blockMap, GameObjectInitaliser gameObjectInitaliser, FireDamageBehaviour fireDamageBehaviour, TextureLoader textureLoader) : base(blockMap, gameObjectInitaliser)
        {
            _fireDamageBehaviour = fireDamageBehaviour;
            _textureLoader = textureLoader;
        }

        protected override void OnLoad()
        {
            if (BlockMap.BlockIsEmpty(Position) ||
                BlockMap.BlockMatches(Position, b => b.FireFuel == 0) ||
                BlockMap.BlockMatches(Position + Vector3.UnitY, b => b?.GetType() == typeof(WaterBlock)))
            {
                Destroy();
                return;
            }

            var vertices = Vertices.ToList();
            var uv = TextureCoords.ToList();
            // if block above is empty, create a tall fire
            if (BlockMap.BlockIsEmpty(Position + Vector3.UnitY))
            {
                var v = Vertices.Select(q => q + Vector3.UnitY).ToArray();
                vertices.AddRange(v);
                uv.AddRange(TextureCoords);
            }

            _modelComponent = new ModelComponent(this)
            {
                Vertices = vertices.ToArray(),
                Texture = new AnimatedModelTexture(new[]
                    {
                        _textureLoader.GetTexture("atlas.png", TextureName.Fire1, uv.ToArray()),
                        _textureLoader.GetTexture("atlas.png", TextureName.Fire2, uv.ToArray()),
                        _textureLoader.GetTexture("atlas.png", TextureName.Fire3, uv.ToArray()),
                        _textureLoader.GetTexture("atlas.png", TextureName.Fire4, uv.ToArray()),
                        _textureLoader.GetTexture("atlas.png", TextureName.Fire5, uv.ToArray()),
                        _textureLoader.GetTexture("atlas.png", TextureName.Fire6, uv.ToArray()),
                        _textureLoader.GetTexture("atlas.png", TextureName.Fire7, uv.ToArray()),
                        _textureLoader.GetTexture("atlas.png", TextureName.Fire8, uv.ToArray()),
                    })
                {
                    UpdateIntervalMs = 0.1,
                }
            };
            Components.Add(_modelComponent);
            _particleComponent = new ParticleComponent(this, new Vector3(0.5f, 1, 0.5f), new FireParticleBehaviour())
            {
                Scale = 0.3f,
            };
            Components.Add(_particleComponent);
        }

        protected override void OnUpdate(ICamera camera, FrameUpdateInfo frameInfo)
        {
            if (!_fireStopped)
            {
                var block = BlockMap.GetBlock(Position); // TODO cache
                if (block == null)
                {
                    _fireStopped = true;
                }
                else
                {
                    if (block.FireFuel <= 0)
                    {
                        _fireDamageBehaviour.FireDamageBlock(block);
                        Logger.Debug($"Fire {Position} ran out of fuel.");
                        _fireStopped = true;
                    }
                    else
                    {
                        // TODO spread vertically
                        SpreadFire(Position + Vector3.UnitX, (float)frameInfo.TimeDelta);
                        SpreadFire(Position - Vector3.UnitX, (float)frameInfo.TimeDelta);
                        SpreadFire(Position + Vector3.UnitZ, (float)frameInfo.TimeDelta);
                        SpreadFire(Position - Vector3.UnitZ, (float)frameInfo.TimeDelta);

                        SpreadFire(Position + Vector3.UnitX + Vector3.UnitY, (float)frameInfo.TimeDelta);
                        SpreadFire(Position - Vector3.UnitX + Vector3.UnitY, (float)frameInfo.TimeDelta);
                        SpreadFire(Position + Vector3.UnitZ + Vector3.UnitY, (float)frameInfo.TimeDelta);
                        SpreadFire(Position - Vector3.UnitZ + Vector3.UnitY, (float)frameInfo.TimeDelta);

                        SpreadFire(Position + Vector3.UnitX - Vector3.UnitY, (float)frameInfo.TimeDelta);
                        SpreadFire(Position - Vector3.UnitX - Vector3.UnitY, (float)frameInfo.TimeDelta);
                        SpreadFire(Position + Vector3.UnitZ - Vector3.UnitY, (float)frameInfo.TimeDelta);
                        SpreadFire(Position - Vector3.UnitZ - Vector3.UnitY, (float)frameInfo.TimeDelta);

                        block.FireFuel -= (float)(block.Temperature * frameInfo.TimeDelta); // TODO different rates for different temps
                    }
                }
            }
            else
            {
                _particleComponent.ParticleSystem.Stop();
                _modelComponent.Invisible = true;

                // only destroy the game object when the particles are finihsed generating
                if (_particleComponent.ParticleSystem.Finished)
                {
                    Destroy();
                }
            }
        }

        private void SpreadFire(Vector3 position, float timeDelta)
        {
            // only spread to block without a block on top
            // maybe spread to leaves and certain other blocks?
            var blockAbove = BlockMap.GetBlock(position + Vector3.UnitY);
            var block = BlockMap.GetBlock(position);
            if (blockAbove != null || block == null)
            {
                return;
            }

            block.Temperature += 10 * timeDelta;

            if (!block.IsOnFire && block.FireFuel > 0 && block.Temperature > block.SetOnFireTemperature)
            {
                block.IsOnFire = true;
                GameObjectInitaliser.Create<Fire>(block.Position);
                // GameObject.Create<Fire>(block.Position); //TODO replace with inject factory func
            }
        }
    }
}
