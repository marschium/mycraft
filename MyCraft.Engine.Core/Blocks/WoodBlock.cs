using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyCraft.Assets;
using MyCraft.Engine.Abstract;

namespace MyCraft.Engine.Core.Blocks
{
    public class WoodBlock : Block
    {
        public override TextureName BackFaceTexture => TextureName.Wood;

        public override TextureName FrontFaceTexture => TextureName.Wood;

        public override TextureName LeftFaceTexture => TextureName.Wood;

        public override TextureName RightFaceTexture => TextureName.Wood;

        public override TextureName TopFaceTexture => TextureName.Wood;

        public override TextureName BottomFaceTexture => TextureName.Wood;
    }
}
