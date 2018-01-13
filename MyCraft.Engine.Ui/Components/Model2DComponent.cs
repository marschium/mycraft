using MyCraft.Assets;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCraft.Engine.Ui.Components
{
    /// <summary>
    /// Class to provider common functionality for drawing textures model to the UI.
    /// </summary>
    public abstract class Model2DComponent : UiComponent
    {
        protected readonly TextureLoader textureLoader;

        public Model2DComponent(Vector2 position, TextureLoader textureLoader) : base(position)
        {
            this.textureLoader = textureLoader;
        }

        public override int TextureId => textureLoader.GetTextureId("atlas.png");

        public override void Load()
        {
            SetBuffers();
        }

        protected void SetBuffers()
        {
            TextureBufferId = GL.GenBuffer();
            VertexBufferId = GL.GenBuffer();

            // Gen buffer for vertices
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferId);
            GL.BufferData(BufferTarget.ArrayBuffer, Vertices.Length * 2 * sizeof(float), Vertices, BufferUsageHint.StaticDraw);

            // TODO probably need a flag for the shader to indicate if texture should be flipped or not
            // Gen buffer for block texture array
            GL.BindBuffer(BufferTarget.ArrayBuffer, TextureBufferId);
            GL.BufferData(BufferTarget.ArrayBuffer, TextureCoords.Length * 2 * sizeof(float), TextureCoords.Select(v => new Vector2(v.X, v.Y * -1)).ToArray(), BufferUsageHint.StaticDraw);
        }
    }
}
