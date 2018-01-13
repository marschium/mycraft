using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyCraft.Assets;

namespace MyCraft.Engine.Core.Blocks
{
    public class StoneWallBlock : Block
    {
        public override TextureName BackFaceTexture => TextureName.StoneWall;

        public override TextureName FrontFaceTexture => TextureName.StoneWall;

        public override TextureName LeftFaceTexture => TextureName.StoneWall;

        public override TextureName RightFaceTexture => TextureName.StoneWall;

        public override TextureName TopFaceTexture => TextureName.StoneWall;

        public override TextureName BottomFaceTexture => TextureName.StoneWall;
    }
}
