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
    public class BurntGrassBlock : Block
    {
        public override TextureName BackFaceTexture => TextureName.BurntGrassSide;

        public override TextureName FrontFaceTexture => TextureName.BurntGrassSide;

        public override TextureName LeftFaceTexture => TextureName.BurntGrassSide;

        public override TextureName RightFaceTexture => TextureName.BurntGrassSide;

        public override TextureName TopFaceTexture => TextureName.BurntGrassTop;

        public override TextureName BottomFaceTexture => TextureName.BurntGrassTop;

        public override float SetOnFireTemperature => float.MaxValue; // TODO is flammable property

        public override float FireFuel { get; set; } = 0;
    }
}
