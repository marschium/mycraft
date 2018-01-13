using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyCraft.Assets;
using MyCraft.Engine.Abstract;
using OpenTK;

namespace MyCraft.Engine.Core.Blocks
{
    public class GrassBlock : Block
    {
        public override TextureName BackFaceTexture => TextureName.GrassSide;

        public override TextureName FrontFaceTexture => TextureName.GrassSide;

        public override TextureName LeftFaceTexture => TextureName.GrassSide;

        public override TextureName RightFaceTexture => TextureName.GrassSide;

        public override TextureName TopFaceTexture => TextureName.GrassTop;

        public override TextureName BottomFaceTexture => TextureName.Dirt;

        public override float FireFuel { get; set; } = 500;
    }
}
