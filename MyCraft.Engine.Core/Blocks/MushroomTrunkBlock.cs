using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyCraft.Assets;

namespace MyCraft.Engine.Core.Blocks
{
    public class MushroomTrunkBlock : Block
    {
        public override TextureName BackFaceTexture => TextureName.MushroomTrunk;

        public override TextureName FrontFaceTexture => TextureName.MushroomTrunk;

        public override TextureName LeftFaceTexture => TextureName.MushroomTrunk;

        public override TextureName RightFaceTexture => TextureName.MushroomTrunk;

        public override TextureName TopFaceTexture => TextureName.MushroomTrunk;

        public override TextureName BottomFaceTexture => TextureName.MushroomTrunk;
    }
}
