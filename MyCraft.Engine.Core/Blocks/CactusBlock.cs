using MyCraft.Assets;
using MyCraft.Util;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCraft.Engine.Core.Blocks
{
    public class CactusBlock : Block
    {
        public CactusBlock()
        {
            IsTransparent = true;
            BackFace = BackFace.Add(new Vector3(0, 0, 0.1f));
            FrontFace = FrontFace.Subtract(new Vector3(0, 0, 0.1f));
            RightFace = RightFace.Subtract(new Vector3(0.1f, 0, 0));
            LeftFace = LeftFace.Add(new Vector3(0.1f, 0, 0));
            TopFace = new Vector3[]
            {
                // top face
                new Vector3(0.1f, 1, 0.1f),
                new Vector3(0.1f, 1, 0.95f),
                new Vector3(0.95f, 1, 0.95f),
                new Vector3(0.1f, 1, 0.1f),
                new Vector3(0.95f, 1, 0.95f),
                new Vector3(0.95f, 1, 0.1f),
            };
            BottomFace = new Vector3[]
            {
                // bottom face
                new Vector3(0.1f, 0.1f, 0.1f),
                new Vector3(0.95f, 0.1f, 0.1f),
                new Vector3(0.95f, 0.1f, 0.95f),
                new Vector3(0.1f, 0.1f, 0.1f),
                new Vector3(0.95f, 0.1f, 0.95f),
                new Vector3(0.1f, 0.1f, 0.95f),
            };
        }

        public override float SetOnFireTemperature => 50f;

        public override TextureName BackFaceTexture => TextureName.CactusSide;

        public override TextureName FrontFaceTexture => TextureName.CactusSide;

        public override TextureName LeftFaceTexture => TextureName.CactusSide;

        public override TextureName RightFaceTexture => TextureName.CactusSide;

        public override TextureName TopFaceTexture => TextureName.CactusTop;

        public override TextureName BottomFaceTexture => TextureName.CactusTop;
    }
}
