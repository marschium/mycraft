using MyCraft.Assets;
using MyCraft.Engine.Core.Items;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCraft.Engine.Ui.Components.InventoryComponents
{
    // TODO load a specific texture for the background of the items bar
    public class ItemsBar : Model2DComponent
    {
        private static float iconSize = 32f;
        private static readonly Vector2[] _itemVertices =
        {
            new Vector2(0f, 0f),
            new Vector2(iconSize, 0f),
            new Vector2(iconSize, iconSize),
            new Vector2(0f, 0f),
            new Vector2(iconSize, iconSize),
            new Vector2(0f, iconSize),
        };

        private static readonly Vector2[] _unscaledTextureCoords =
        {
                new Vector2(1f,1f),
                new Vector2(0f,1f),
                new Vector2(0f,0f),
                new Vector2(1f,1f),
                new Vector2(0f,0f),
                new Vector2(1f,0f),
        };

        private readonly Inventory _inventory;
        private readonly TextureLoader _textureLoader;

        private Vector2[] _vertices;
        private Vector2[] _textureCoords;
        private bool _generatedBuffers;

        public ItemsBar(Vector2 position, Inventory inventory, TextureLoader textureLoader) : base(position, textureLoader)
        {
            _inventory = inventory;
            _textureLoader = textureLoader;
        }

        public override Vector2[] Vertices {
            get
            {
                if (!_generatedBuffers)
                {
                    BuildBuffers();
                }
                return _vertices;
            }
        }

        public override Vector2[] TextureCoords
        {
            get
            {
                if (!_generatedBuffers)
                {
                    BuildBuffers();
                }
                return _textureCoords;
            }
        }

        private void BuildBuffers()
        {
            //_texture = _textureLoader.GetTexture(TextureName.Outline, _textureCoords);
            _vertices = new Vector2[_inventory.MaxItems * _itemVertices.Length];
            _textureCoords = new Vector2[_inventory.MaxItems * _itemVertices.Length];
            TextureId = textureLoader.GetTextureId("atlas.png");
            int i = -1;
            for (int v = 0; v < _vertices.Length; v++)
            {
                var vertexIndex = v % _itemVertices.Length;
                var itemIndex = v / _itemVertices.Length;
                var vertexToCopy = _itemVertices[vertexIndex];
                _vertices[v] = new Vector2(vertexToCopy.X + (itemIndex * iconSize), vertexToCopy.Y);
                if (i != itemIndex)
                {
                    i = itemIndex; // if item index has changed get more scaled texture coords
                    var t = _textureLoader.GetScaledUVCoords("atlas.png", _inventory[itemIndex]?.InventoryIconTexture ?? TextureName.Debug, _unscaledTextureCoords);
                    t.CopyTo(_textureCoords, i * t.Length);
                }
            }
            _generatedBuffers = true;
        }
    }
}
