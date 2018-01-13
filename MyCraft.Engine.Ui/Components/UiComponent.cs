using MyCraft.Assets;
using MyCraft.Engine.Abstract;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCraft.Engine.Ui.Components
{
    public abstract class UiComponent
    {
        public UiComponent(Vector2 position)
        {
            Position = position;
        }

        public virtual Vector2 Position { get; set; }

        public virtual Vector2[] Vertices { get; protected set; }

        public virtual int VertexBufferId { get; protected set; }

        public virtual Vector2[] TextureCoords { get; protected set; }

        public virtual int TextureBufferId { get; protected set; }

        public virtual int TextureId { get; protected set; }

        public abstract void Load();

        public virtual void Update(FrameUpdateInfo frameInfo)
        {
        }
    }
}
