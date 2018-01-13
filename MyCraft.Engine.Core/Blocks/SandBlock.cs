using MyCraft.Assets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCraft.Engine.Core.Blocks
{
    public class SandBlock : Block
    {
        public override TextureName BackFaceTexture => TextureName.Sand;

        public override TextureName FrontFaceTexture => TextureName.Sand;

        public override TextureName LeftFaceTexture => TextureName.Sand;

        public override TextureName RightFaceTexture => TextureName.Sand;

        public override TextureName TopFaceTexture => TextureName.Sand;

        public override TextureName BottomFaceTexture => TextureName.Sand;
    }
}
