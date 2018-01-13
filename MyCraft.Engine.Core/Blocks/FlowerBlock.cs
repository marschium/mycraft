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
    public class FlowerBlock : Block
    {
        public FlowerBlock()
        {
            Collision = false;
            IsTransparent = true;
        }

        public override TextureName BackFaceTexture => TextureName.Grass;
        public override TextureName FrontFaceTexture => TextureName.Grass;
        public override TextureName LeftFaceTexture => TextureName.Grass;
        public override TextureName RightFaceTexture => TextureName.Grass;
        public override TextureName TopFaceTexture => TextureName.Grass;
        public override TextureName BottomFaceTexture => TextureName.Grass;

        public override Vector3[] BackFace => new Vector3[]
        {
               new Vector3(0,0,0),
               new Vector3(0,1,0),
               new Vector3(1,1,1),
               new Vector3(0,0,0),
               new Vector3(1,1,1),
               new Vector3(1,0,1),
        };
        public override Vector3[] FrontFace => new Vector3[]
        {
            new Vector3(0,0,0),
            new Vector3(1,0,1),
            new Vector3(1,1,1),
            new Vector3(0,0,0),
            new Vector3(1,1,1),
            new Vector3(0,1,0),
        };
        public override Vector3[] RightFace => new Vector3[]
        {
               new Vector3(1,0,0),
               new Vector3(1,1,0),
               new Vector3(0,1,1),
               new Vector3(1,0,0),
               new Vector3(0,1,1),
               new Vector3(0,0,1),
        };
        public override Vector3[] LeftFace => new Vector3[]
        {
               new Vector3(1,0,0),
               new Vector3(0,0,1),
               new Vector3(0,1,1),
               new Vector3(1,0,0),
               new Vector3(0,1,1),
               new Vector3(1,1,0),
        };
        public override Vector3[] TopFace => new Vector3[] { };
        public override Vector3[] BottomFace => new Vector3[] { };

        public override Vector2[] BackFaceUV => base.BackFaceUV;
        public override Vector2[] FrontFaceUV => base.FrontFaceUV;
        public override Vector2[] RightFaceUV => base.RightFaceUV;
        public override Vector2[] LeftFaceUV => base.LeftFaceUV;
        public override Vector2[] TopFaceUV => new Vector2[] { };
        public override Vector2[] BottomFaceUV => new Vector2[] { };
    }
}
