using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using MyCraft.Util;
using MyCraft.Engine.Ui.Components;
using MyCraft.Assets;
using MyCraft.Engine.Abstract;
using MyCraft.Engine.GameObjects;

namespace MyCraft.Engine.Ui
{
    public class GameInterface
    {
        // 800 X 600 so that we can render properly at any resolution
        private const int Width = 800;
        private const int Height = 600;

        private UIRenderer _uiRenderer;
        private TextureLoader _textureLoader;
        private readonly Player _player;
        private TextUtil _textUtil;

        private DebugComponent _debugComponent;
        private InventoryHotbarComponent _inventoryHotbarComponent;
        
        public GameInterface(ShaderLoader shaderLoader, TextureLoader textureLoader, Player player)
        {
            _uiRenderer = new UIRenderer(shaderLoader);
            _textureLoader = textureLoader;
            _player = player;
            _textUtil = new TextUtil();
            _debugComponent = new DebugComponent(player, _textUtil, new Vector2(10, 100));
            _inventoryHotbarComponent = new InventoryHotbarComponent(new Vector2(100, 10), player.Items, textureLoader);
        }

        public FilterFlags FilterFlags { get; set; }

        public void Load()
        {
            _debugComponent.Load();
            _inventoryHotbarComponent.Load();
        }

        public void Update(FrameUpdateInfo frameInfo)
        {
            _debugComponent.Update(frameInfo);
            _inventoryHotbarComponent.Update(frameInfo);
        }

        public void Render()
        {            
            _uiRenderer.Draw(_debugComponent);
            _uiRenderer.Draw(_inventoryHotbarComponent.SelectedItem);
            _uiRenderer.Draw(_inventoryHotbarComponent.Items);
        }
    }
}
