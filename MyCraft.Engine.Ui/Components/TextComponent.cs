using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyCraft.Assets;
using MyCraft.Engine.Abstract;
using MyCraft.Engine.Core.GameObjects.Components;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace MyCraft.Engine.Ui.Components
{
    public class TextComponent : UiComponent
    {
        private readonly TextUtil _textUtil;
        private string _text;
        private Text _textInfo;

        public TextComponent(string text, TextUtil textUtil, Vector2 position) : base(position)
        {
            _text = text;
            _textUtil = textUtil;
        }

        public string Contents {
            get
            {
                return _text;
            }
            set
            {
                _text = value;
                Unload();
                Load();
            }
        }

        public override Vector2[] Vertices => _textInfo.Vertices;

        public override int VertexBufferId => _textInfo.VertexBufferId;

        public override Vector2[] TextureCoords => _textInfo.Texture.Coords;

        public override int TextureBufferId => _textInfo.Texture.TextureBufferId;

        public override int TextureId => _textInfo.Texture.TextureId;

        public override void Load()
        {
            _textInfo = _textUtil.GetText(_text, 8);
            SetBuffers();
        }

        private void SetBuffers()
        {
            _textInfo.Texture.TextureBufferId = GL.GenBuffer();
            _textInfo.VertexBufferId = GL.GenBuffer();

            // Gen buffer for vertices
            GL.BindBuffer(BufferTarget.ArrayBuffer, _textInfo.VertexBufferId);
            GL.BufferData(BufferTarget.ArrayBuffer, _textInfo.Vertices.Length * 2 * sizeof(float), _textInfo.Vertices, BufferUsageHint.StaticDraw);

            // Gen buffer for block texture array
            GL.BindBuffer(BufferTarget.ArrayBuffer, _textInfo.Texture.TextureBufferId);
            GL.BufferData(BufferTarget.ArrayBuffer, _textInfo.Texture.Coords.Length * 2 * sizeof(float), _textInfo.Texture.Coords, BufferUsageHint.StaticDraw);
        }

        public void Unload()
        {
            GL.DeleteBuffers(2, new[] { _textInfo.VertexBufferId, _textInfo.Texture.TextureBufferId });
            _textInfo = null;
        }
    }
}
