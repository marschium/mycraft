using MyCraft.Engine.Abstract;
using MyCraft.Engine.Core.GameObjects.Components;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCraft.Engine.GameObjects.Components
{
    public class ModelComponent : AbstractComponent
    {
        public ModelComponent(GameObject parent) : base(parent)
        {
        }

        public ModelComponent(GameObject parent, Vector3 offset) : base(parent, offset)
        {
        }

        public Vector3[] Vertices { get; set; }

        public Texture Texture { get; set; }

        public int VertexBufferId { get; set; }

        public override void Load()
        {
            // Gen buffer for vertices
            VertexBufferId = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferId);
            GL.BufferData(BufferTarget.ArrayBuffer, Vertices.Length * 3 * sizeof(float), Vertices, BufferUsageHint.StaticDraw);

            Texture?.Load();
        }

        public override void Update(FrameUpdateInfo frameInfo)
        {
            base.Update(frameInfo);
            Texture.Update(frameInfo);
        }
    }
}
