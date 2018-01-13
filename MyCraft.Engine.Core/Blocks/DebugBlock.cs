using MyCraft.Assets;
using MyCraft.Engine.Abstract;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCraft.Engine.Core.Blocks
{
    public class DebugBlock : Block
    {
        public override TextureName BackFaceTexture => TextureName.Water;

        public override TextureName FrontFaceTexture => TextureName.Water;

        public override TextureName LeftFaceTexture => TextureName.Water;

        public override TextureName RightFaceTexture => TextureName.Water;

        public override TextureName TopFaceTexture => TextureName.Water;

        public override TextureName BottomFaceTexture => TextureName.Water;
    }
}
