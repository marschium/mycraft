using MyCraft.Assets;
using MyCraft.Engine.Abstract;
using MyCraft.Engine.Core.GameObjects;
using MyCraft.Engine.Core.GameObjects.Components;
using MyCraft.Engine.GameObjects.Camera;
using MyCraft.Engine.GameObjects.Components;
using MyCraft.Engine.World;
using NLog;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCraft.Engine.GameObjects
{
    /// <summary>
    /// Drawn around the currently selected block
    /// </summary>
    public class SelectedBlock : GameObject
    {
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
            // top face
            new Vector3(-0.1f, 1.1f, -0.1f),
            new Vector3(-0.1f, 1.1f, 1.1f),
            new Vector3(1.1f, 1.1f, 1.1f),
            new Vector3(-0.1f, 1.1f, -0.1f),
            new Vector3(1.1f, 1.1f, 1.1f),
            new Vector3(1.1f, 1.1f, -0.1f),
            // bottom face
            new Vector3(-0.1f, -0.1f, -0.1f),
            new Vector3(1.1f, -0.1f, -0.1f),
            new Vector3(1.1f, -0.1f, 1.1f),
            new Vector3(-0.1f, -0.1f, -0.1f),
            new Vector3(1.1f, -0.1f, 1.1f),
            new Vector3(-0.1f, -0.1f, 1.1f),
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

        public SelectedBlock(BlockMap blockMap, GameObjectInitaliser gameObjectInitalizer, TextureLoader TextureLoader) : base (blockMap, gameObjectInitalizer)
        {
            AffectedByGravity = false;
            IgnoreCollision = true;
            _textureLoader = TextureLoader;
        }

        public int VertexBufferId { get; private set; }

        public int TextureBufferId { get; private set; }

        public Vector2[] ScaledTextureCoords { get; private set; }
        
        protected override void OnUpdate(ICamera camera, FrameUpdateInfo frameInfo)
        {
        }

        protected override void OnLoad()
        {
            Components.Add(new ModelComponent(this)
            {
                Vertices = Vertices,
                Texture = _textureLoader.GetTexture("atlas.png", TextureName.Outline, TextureCoords)
            });
        }
    }
}
