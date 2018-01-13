using MyCraft.Engine.Abstract;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCraft.Engine.Core.GameObjects.Components
{
    public class Texture
    {
        public int TextureBufferId { get; set; }

        public Vector2[] Coords { get; set; }

        public int TextureId { get; set; }

        public void Load()
        {
            // Gen buffer for block texture array
            TextureBufferId = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, TextureBufferId);
            GL.BufferData(BufferTarget.ArrayBuffer, Coords.Length * 2 * sizeof(float), Coords, BufferUsageHint.StaticDraw);
        }

        public virtual void Update(FrameUpdateInfo frameUpdateInfo)
        {
        }
    }
}
