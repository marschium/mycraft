using MyCraft.Assets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCraft.Engine.Core.Blocks
{
    public class SmallMushroomBlock : FlowerBlock
    {
        public override TextureName BackFaceTexture => TextureName.SmallMushroom;
        public override TextureName FrontFaceTexture => TextureName.SmallMushroom;
        public override TextureName LeftFaceTexture => TextureName.SmallMushroom;
        public override TextureName RightFaceTexture => TextureName.SmallMushroom;
        public override TextureName TopFaceTexture => TextureName.SmallMushroom;
        public override TextureName BottomFaceTexture => TextureName.SmallMushroom;
    }
}
