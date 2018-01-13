using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyCraft.Assets;

namespace MyCraft.Engine.Core.Blocks
{
    public class WoodPlanksBlock : Block
    {
        public override TextureName BackFaceTexture => TextureName.WoodPlanks;

        public override TextureName FrontFaceTexture => TextureName.WoodPlanks;

        public override TextureName LeftFaceTexture => TextureName.WoodPlanks;

        public override TextureName RightFaceTexture => TextureName.WoodPlanks;

        public override TextureName TopFaceTexture => TextureName.WoodPlanks;

        public override TextureName BottomFaceTexture => TextureName.WoodPlanks;
    }
}
