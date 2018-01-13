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
    public class StoneBlock : Block
    {
        public override TextureName BackFaceTexture => TextureName.Stone;

        public override TextureName FrontFaceTexture => TextureName.Stone;

        public override TextureName LeftFaceTexture => TextureName.Stone;

        public override TextureName RightFaceTexture => TextureName.Stone;

        public override TextureName TopFaceTexture => TextureName.Stone;

        public override TextureName BottomFaceTexture => TextureName.Stone;
    }
}
