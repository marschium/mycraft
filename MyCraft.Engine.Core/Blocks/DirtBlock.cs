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
    public class DirtBlock : Block
    {
        public override TextureName BackFaceTexture => TextureName.Dirt;

        public override TextureName FrontFaceTexture => TextureName.Dirt;

        public override TextureName LeftFaceTexture => TextureName.Dirt;

        public override TextureName RightFaceTexture => TextureName.Dirt;

        public override TextureName TopFaceTexture => TextureName.Dirt;
        
        public override TextureName BottomFaceTexture => TextureName.Dirt;
    }
}
