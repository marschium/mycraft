using MyCraft.Assets;
using MyCraft.Engine.Abstract;
using MyCraft.Engine.Core.GameObjects.Components;
using MyCraft.Engine.GameObjects;
using MyCraft.Engine.GameObjects.Camera;
using MyCraft.Engine.GameObjects.Components;
using MyCraft.Engine.World;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCraft.Engine.Core.GameObjects
{
    public class Cloud : GameObject
    {
        private static int minSizeX = 2;
        private static int minSizeZ = 10;
        private static int maxSize = 20;

        public float speed = 0.5f;

        public static Vector3[] Vertices = new Vector3[]
        {
            // front face
            new Vector3(-0.5f, -0.25f, 0.5f),
            new Vector3(0.5f, -0.25f, 0.5f),
            new Vector3(0.5f, 0.25f, 0.5f),
            new Vector3(-0.5f, -0.25f, 0.5f),
            new Vector3(0.5f, 0.25f, 0.5f),
            new Vector3(-0.5f, 0.25f, 0.5f),
            // back face
            new Vector3(-0.5f, -0.25f, -0.5f),
            new Vector3(-0.5f, 0.25f, -0.5f),
            new Vector3(0.5f, 0.25f, -0.5f),
            new Vector3(-0.5f, -0.25f, -0.5f),
            new Vector3(0.5f, 0.25f, -0.5f),
            new Vector3(0.5f, -0.25f, -0.5f),
            // right face
            new Vector3(0.5f, -0.25f, -0.5f),
            new Vector3(0.5f, 0.25f, -0.5f),
            new Vector3(0.5f, 0.25f, 0.5f),
            new Vector3(0.5f, -0.25f, -0.5f),
            new Vector3(0.5f, 0.25f, 0.5f),
            new Vector3(0.5f, -0.25f, 0.5f),
            // left face
            new Vector3(-0.5f, -0.25f, -0.5f),
            new Vector3(-0.5f, -0.25f, 0.5f),
            new Vector3(-0.5f, 0.25f, 0.5f),
            new Vector3(-0.5f, -0.25f, -0.5f),
            new Vector3(-0.5f, 0.25f, 0.5f),
            new Vector3(-0.5f, 0.25f, -0.5f),
            // top face
            new Vector3(-0.5f, 0.25f, -0.5f),
            new Vector3(-0.5f, 0.25f, 0.5f),
            new Vector3(0.5f, 0.25f, 0.5f),
            new Vector3(-0.5f, 0.25f, -0.5f),
            new Vector3(0.5f, 0.25f, 0.5f),
            new Vector3(0.5f, 0.25f, -0.5f),
            // bottom face
            new Vector3(-0.5f, -0.25f, -0.5f),
            new Vector3(0.5f, -0.25f, -0.5f),
            new Vector3(0.5f, -0.25f, 0.5f),
            new Vector3(-0.5f, -0.25f, -0.5f),
            new Vector3(0.5f, -0.25f, 0.5f),
            new Vector3(-0.5f, -0.25f, 0.5f),
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

        private TextureLoader _textureLoader;

        public Cloud(BlockMap blockMap, GameObjectInitaliser gameObjectInitaliser, TextureLoader textureLoader) : base(blockMap, gameObjectInitaliser)
        {
            _textureLoader = textureLoader;
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            var r = new Random();
            var xs = r.Next(minSizeX, maxSize);
            var zs = r.Next(minSizeZ, maxSize);
            var texture = _textureLoader.GetTexture("atlas.png", TextureName.Cloud, TextureCoords);
            Components.Add(new ModelComponent(this)
            {
                Vertices = Vertices.Select(v => new Vector3(v.X * xs, v.Y, v.Z * zs)).ToArray(),
                Texture = texture,
            });
        }

        protected override void OnUpdate(ICamera camera, FrameUpdateInfo frameInfo)
        {
            base.OnUpdate(camera, frameInfo);
            deltaMovement.Z = speed;
        }
    }
}
