using MyCraft.Assets;
using MyCraft.Engine.Abstract;
using MyCraft.Engine.GameObjects;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCraft.Engine.Core.Blocks
{
    public abstract class Block
    {

        public virtual Vector3[] BackFace { get; protected set; } = BlockFaces.BackFace;
        public virtual Vector3[] FrontFace { get; protected set; } = BlockFaces.FrontFace;
        public virtual Vector3[] RightFace { get; protected set; } = BlockFaces.RightFace;
        public virtual Vector3[] LeftFace { get; protected set; } = BlockFaces.LeftFace;
        public virtual Vector3[] TopFace { get; protected set; } = BlockFaces.TopFace;
        public virtual Vector3[] BottomFace { get; protected set; } = BlockFaces.BottomFace;

        public virtual Vector2[] BackFaceUV => BlockFaces.FaceTextureCoords;
        public virtual Vector2[] FrontFaceUV => BlockFaces.FrontFaceTextureCoords; // TODO invert x axis for all front textures
        public virtual Vector2[] RightFaceUV => BlockFaces.FaceTextureCoords;
        public virtual Vector2[] LeftFaceUV => BlockFaces.FrontFaceTextureCoords;
        public virtual Vector2[] TopFaceUV => BlockFaces.FaceTextureCoords;
        public virtual Vector2[] BottomFaceUV => BlockFaces.FaceTextureCoords;

        public abstract TextureName BackFaceTexture { get; }
        public abstract TextureName FrontFaceTexture { get; }
        public abstract TextureName LeftFaceTexture { get; }
        public abstract TextureName RightFaceTexture { get; }
        public abstract TextureName TopFaceTexture { get; }
        public abstract TextureName BottomFaceTexture { get; }

        public Vector3 Position { get; set; }

        public bool IsTransparent { get; protected set; } = false;

        public virtual float SetOnFireTemperature { get; protected set; } = 100;

        public virtual float FireDamageTemperature { get; protected set; } = 150;

        public virtual float SetOnFireRatio { get; protected set; } = 0.75f;

        public virtual float FireFuel { get; set; } = 500;// 1 if 1C for 1 Sec

        public bool IsOnFire { get; set; }

        public virtual float Temperature { get; set; } = 20f;

        public byte Light { get; set; } // 256 is high, 0 is low light. light level of block

        public bool IsFluid { get; set; } = false;

        public bool Collision { get; set; } = true;

        public IUpdateScheduler Scheduler { get; set; }
    }
}
