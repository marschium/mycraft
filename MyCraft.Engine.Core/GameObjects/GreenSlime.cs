using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyCraft.Engine.Abstract;
using MyCraft.Engine.GameObjects.Camera;
using OpenTK;
using MyCraft.Assets;
using MyCraft.Engine.GameObjects.Ai;
using MyCraft.Engine.GameObjects.Ai.Actions;
using MyCraft.Engine.World;
using MyCraft.Engine.GameObjects.Components;
using MyCraft.Engine.Particles.Behaviours;
using MyCraft.Engine.Particles;
using MyCraft.Engine.Core.GameObjects.Components;
using MyCraft.Engine.Core.GameObjects;

namespace MyCraft.Engine.GameObjects
{
    public class GreenSlime : GameObject
    {
        public static Vector3[] Vertices = new Vector3[]
        {
            // front face
            new Vector3(-0.6f, -0.6f, 0.6f),
            new Vector3(0.6f, -0.6f, 0.6f),
            new Vector3(0.6f, 0.6f, 0.6f),
            new Vector3(-0.6f, -0.6f, 0.6f),
            new Vector3(0.6f, 0.6f, 0.6f),
            new Vector3(-0.6f, 0.6f, 0.6f),
            // back face
            new Vector3(-0.6f, -0.6f, -0.6f),
            new Vector3(-0.6f, 0.6f, -0.6f),
            new Vector3(0.6f, 0.6f, -0.6f),
            new Vector3(-0.6f, -0.6f, -0.6f),
            new Vector3(0.6f, 0.6f, -0.6f),
            new Vector3(0.6f, -0.6f, -0.6f),
            // right face
            new Vector3(0.6f, -0.6f, -0.6f),
            new Vector3(0.6f, 0.6f, -0.6f),
            new Vector3(0.6f, 0.6f, 0.6f),
            new Vector3(0.6f, -0.6f, -0.6f),
            new Vector3(0.6f, 0.6f, 0.6f),
            new Vector3(0.6f, -0.6f, 0.6f),
            // left face
            new Vector3(-0.6f, -0.6f, -0.6f),
            new Vector3(-0.6f, -0.6f, 0.6f),
            new Vector3(-0.6f, 0.6f, 0.6f),
            new Vector3(-0.6f, -0.6f, -0.6f),
            new Vector3(-0.6f, 0.6f, 0.6f),
            new Vector3(-0.6f, 0.6f, -0.6f),
            // top face
            new Vector3(-0.6f, 0.6f, -0.6f),
            new Vector3(-0.6f, 0.6f, 0.6f),
            new Vector3(0.6f, 0.6f, 0.6f),
            new Vector3(-0.6f, 0.6f, -0.6f),
            new Vector3(0.6f, 0.6f, 0.6f),
            new Vector3(0.6f, 0.6f, -0.6f),
            // bottom face
            new Vector3(-0.6f, -0.6f, -0.6f),
            new Vector3(0.6f, -0.6f, -0.6f),
            new Vector3(0.6f, -0.6f, 0.6f),
            new Vector3(-0.6f, -0.6f, -0.6f),
            new Vector3(0.6f, -0.6f, 0.6f),
            new Vector3(-0.6f, -0.6f, 0.6f),

        };

        public static Vector3[] FaceModel = new Vector3[]
        {            
            // slime face

            new Vector3(0.6f, 0.6f, 0.61f),
            new Vector3(-0.6f, -0.6f, 0.61f),
            new Vector3(0.6f, -0.6f, 0.61f),
            new Vector3(0.6f, 0.6f, 0.61f),
            new Vector3(-0.6f, 0.6f, 0.61f),
            new Vector3(-0.6f, -0.6f, 0.61f),

            //new Vector2(1f,1f),
            //new Vector2(0f,0f),
            //new Vector2(1f,0f),
            //// bottom tri
            //new Vector2(1f,1f),
            //new Vector2(0f,1f),
            //new Vector2(0f,0f),
        };

        public static Vector2[] TextureCoords = new Vector2[]
        {
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
            new Vector2(1f,0f),
            new Vector2(0f,0f),
            // bottom tri
            new Vector2(1f,1f),
            new Vector2(0f,0f),
            new Vector2(0f,1f),

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
            new Vector2(1f,0f),
            new Vector2(0f,0f),
            // bottom tri
            new Vector2(1f,1f),
            new Vector2(0f,0f),
            new Vector2(0f,1f),

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
            new Vector2(1f,0f),
            new Vector2(0f,0f),
            // bottom tri
            new Vector2(1f,1f),
            new Vector2(0f,0f),
            new Vector2(0f,1f),
        };

        private static Vector2[] FaceTexture = new Vector2[] {
            // top tri
            new Vector2(0f,0f),
            new Vector2(1f,1f),
            new Vector2(0f,1f),
            // bottom tri
            new Vector2(0f,0f),
            new Vector2(1f,0f),
            new Vector2(1f,1f),
        };

        private TextureLoader _textureLoader;
        private double _timeSinceLastJump = 0;


        public GreenSlime(BlockMap blockMap, GameObjectInitaliser gameObjectInitaliser, TextureLoader textureLoader) : base(blockMap, gameObjectInitaliser)
        {
            _textureLoader = textureLoader;
            AffectedByGravity = true;
            IgnoreCollision = false;
        }

        public GameObjectAction<GreenSlime> CurrentAction { get; private set; }

        protected override void OnLoad()
        {
            var bodyTexture = _textureLoader.GetTexture("atlas.png", TextureName.Slime, TextureCoords);
            var faceTexture = _textureLoader.GetTexture("atlas.png", TextureName.SlimeFace, FaceTexture);
            Components.Add(new ModelComponent(this, Vector3.UnitY / 2)
            {
                Vertices = Vertices.Concat(FaceModel).ToArray(),
                Texture = new Texture { Coords = bodyTexture.Coords.Concat(faceTexture.Coords).ToArray() },
            });
        }

        protected override void OnUpdate(ICamera camera, FrameUpdateInfo frameInfo)
        {
            if (IsOnGround)
            {
                if (_timeSinceLastJump == 0)
                {
                    // slime has just landed
                    // var go = GameObject.Create<GameObject>(Position); TODO replace with factory function or something
                    //var effect = new SlimeTrailParticleBehaviour();
                    //var pc = new ParticleComponent(go, new Vector3(0.5f, 0.05f, 0.5f), effect) { Scale = 0.5f };
                    //pc.ParticleSystem.ParticleOrientation = ParticleOrientation.Floor;
                    //go.Components.Add(pc);
                }
                _timeSinceLastJump += frameInfo.TimeDelta;
                deltaMovement = Vector3.Zero;
            }

            // update the current AiState
            if (CurrentAction != null)
            {
                var finished = CurrentAction.Act(frameInfo);
                if (finished)
                {
                    CurrentAction = null;
                }
            }
            else
            {
                CurrentAction = new SlimeEatBlockAction(this);
            }
        }

        public void Jump()
        {
            if (IsOnGround && _timeSinceLastJump > 1)
            {
                deltaMovement = (Direction  + new Vector3(0, 1.5f, 0)).Normalized() * 30;
                _timeSinceLastJump = 0;
            }
        }

        public void RotateInAir(float rotation)
        {
            if (IsOnGround && _timeSinceLastJump > 1)
            {
                deltaMovement += Vector3.UnitY * 30;
                _timeSinceLastJump = 0;
            }
            if (!IsOnGround)
            {
                HorizontalRotation += rotation;
            }
        }

        public void Slide()
        {
            if (IsOnGround)
            {
                deltaMovement = Direction.Normalized();
            }
        }
    }
}
