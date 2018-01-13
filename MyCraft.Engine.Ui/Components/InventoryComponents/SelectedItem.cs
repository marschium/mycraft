using MyCraft.Assets;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCraft.Engine.Ui.Components
{
    public class SelectedItem : Model2DComponent
    {
        private float _size;
        private Lazy<Vector2[]> _texture;

        private static readonly Vector2[] _textureCoords =
        {
                new Vector2(1f,1f),
                new Vector2(0f,1f),
                new Vector2(0f,0f),
                new Vector2(1f,1f),
                new Vector2(0f,0f),
                new Vector2(1f,0f),
        };

        public SelectedItem(Vector2 position, TextureLoader textureLoader, float size) : base(position, textureLoader)
        {
            _size = size;
            _texture = new Lazy<Vector2[]>(() => textureLoader.GetScaledUVCoords("atlas.png", TextureName.Outline, _textureCoords));
        }

        public override Vector2[] Vertices => new[]
            {
                new Vector2(0f, 0f),
                new Vector2(_size, 0f),
                new Vector2(_size, _size),
                new Vector2(0f, 0f),
                new Vector2(_size, _size),
                new Vector2(0f, _size),
            };

        public override  Vector2[] TextureCoords => _texture.Value;

        public override int TextureId => textureLoader.GetTextureId("atlas.png");
    }
}
