using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCraft.Engine.Abstract
{
    public class FrameRenderInfo
    {
        public int MvpId { get; set; }

        public float Fog { get; set; }

        public Vector3 FogColour { get; set; }
    }
}
