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
    public class LeafBlock : Block
    {
        public LeafBlock()
        {
            IsTransparent = true;
        }

        public override TextureName BackFaceTexture => TextureName.Leaf;

        public override TextureName FrontFaceTexture => TextureName.Leaf;

        public override TextureName LeftFaceTexture => TextureName.Leaf;

        public override TextureName RightFaceTexture => TextureName.Leaf;

        public override TextureName TopFaceTexture => TextureName.Leaf;

        public override TextureName BottomFaceTexture => TextureName.Leaf;

        public override float SetOnFireTemperature => 50f;
    }
}
