using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyCraft.Assets;

namespace MyCraft.Engine.Core.Blocks
{
    public class ExplosiveBlock : Block
    {
        public override TextureName BackFaceTexture => TextureName.Explosive;

        public override TextureName FrontFaceTexture => TextureName.Explosive;

        public override TextureName LeftFaceTexture => TextureName.Explosive;

        public override TextureName RightFaceTexture => TextureName.Explosive;

        public override TextureName TopFaceTexture => TextureName.Explosive;

        public override TextureName BottomFaceTexture => TextureName.Explosive;
    }
}
