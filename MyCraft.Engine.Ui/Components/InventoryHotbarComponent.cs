using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyCraft.Assets;
using MyCraft.Engine.Abstract;
using MyCraft.Engine.Core.GameObjects.Components;
using MyCraft.Engine.Core.Items;
using MyCraft.Engine.GameObjects;
using MyCraft.Engine.Ui.Components.InventoryComponents;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace MyCraft.Engine.Ui.Components
{
    public class InventoryHotbarComponent
    {
        private Inventory _inv;
        private TextureLoader _textureLoader;
        private Vector2 _position;

        public InventoryHotbarComponent(Vector2 position, Inventory inv, TextureLoader textureLoader)
        {
            Items = new ItemsBar(position, inv, textureLoader);
            SelectedItem = new SelectedItem(position, textureLoader, 32);
            _inv = inv;
            _textureLoader = textureLoader;
            _position = position;
        }    
        
        public ItemsBar Items { get; }

        public SelectedItem SelectedItem { get; }

        public void Load()
        {
            // TODO different interfaces for loading and drawing
            Items.Load();
            SelectedItem.Load();
        }

        public void Update(FrameUpdateInfo frameInfo)
        {
            SelectedItem.Position = _position + new Vector2((_inv.CurrentIndex * 32), 0);
        }
    }
}
