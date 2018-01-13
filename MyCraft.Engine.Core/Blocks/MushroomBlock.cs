using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyCraft.Assets;

namespace MyCraft.Engine.Core.Blocks
{
    public class MushroomBlock : Block
    {
        public MushroomBlock()
        {
            IsTransparent = false;
        }

        public override TextureName BackFaceTexture => TextureName.Mushroom;

        public override TextureName FrontFaceTexture => TextureName.Mushroom;

        public override TextureName LeftFaceTexture => TextureName.Mushroom;

        public override TextureName RightFaceTexture => TextureName.Mushroom;

        public override TextureName TopFaceTexture => TextureName.Mushroom;

        public override TextureName BottomFaceTexture => TextureName.MushroomTrunk;
    }
}
