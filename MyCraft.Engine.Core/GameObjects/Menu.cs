using MyCraft.Assets;
using MyCraft.Engine.Abstract;
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
    /// <summary>
    /// A 2d in world menu
    /// </summary>
    public class Menu : GameObject
    {
        private static Vector3[] Vertices =
        {
            new Vector3(-0.5f, -0.5f, 0f),
            new Vector3(0.5f, 0.5f, 0f),
            new Vector3(-0.5f, 0.5f, 0f),
            new Vector3(-0.5f, -0.5f, 0f),
            new Vector3(0.5f, -0.5f, 0f),
            new Vector3(0.5f, 0.5f, 0f),
        };

        private static Vector2[] TextureCoords =
        {
            // DEBUG
            new Vector2(0f,1f),
            new Vector2(1f,0f),
            new Vector2(0f,0f),
            new Vector2(0f,1f),
            new Vector2(1f,1f),
            new Vector2(1f,0f),
        };

        private Player _player;
        private TextureLoader _textureLoader;

        public Menu(BlockMap blockMap, GameObjectInitaliser gameObjectInitaliser, Player player, TextureLoader textureLoader) : base(blockMap, gameObjectInitaliser)
        {
            _player = player;
            _textureLoader = textureLoader;
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            var texture = _textureLoader.GetTexture("slime_maker_menu.png", TextureName.Menu, TextureCoords);
            Components.Add(new ModelComponent(this, new Vector3(0f, 0f, 0f))
            {
                Vertices = Vertices,
                Texture = texture,
            });
        }

        protected override void OnUpdate(ICamera camera, FrameUpdateInfo frameInfo)
        {
            base.OnUpdate(camera, frameInfo);
            var d = new Vector3(_player.Position.X - Position.X, 0f, _player.Position.Z - Position.Z);
            d.Normalize();
            Direction = d;
        }
    }
}
