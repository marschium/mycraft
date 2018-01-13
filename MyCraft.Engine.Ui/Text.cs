using MyCraft.Engine.Core.GameObjects.Components;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCraft.Engine.Ui
{
    public class Text
    {
        public int VertexBufferId { get; set; }

        public Vector2[] Vertices { get; set; }

        public Texture Texture { get; set; }
    }
}
