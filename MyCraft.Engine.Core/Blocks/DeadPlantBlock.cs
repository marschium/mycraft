using MyCraft.Assets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCraft.Engine.Core.Blocks
{
    public class DeadPlantBlock : FlowerBlock
    {
        public override TextureName BackFaceTexture => TextureName.DeadPlant;

        public override TextureName FrontFaceTexture => TextureName.DeadPlant;

        public override TextureName LeftFaceTexture => TextureName.DeadPlant;

        public override TextureName RightFaceTexture => TextureName.DeadPlant;

        public override TextureName TopFaceTexture => TextureName.DeadPlant;

        public override TextureName BottomFaceTexture => TextureName.DeadPlant;
    }
}
