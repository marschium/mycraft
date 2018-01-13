using MyCraft.Util;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCraft.Engine.World
{
    public class WorldUtil
    {
        public static Vector3 FloorToChunkPosition(Vector3 v)
        {
            var r = Chunk.Width * (v / Chunk.Width).Floor();
            r.Y = 0;
            return r;
        }

        public static Vector3 ToBlockPosition(Vector3 v)
        {
            // if position < 0 round up
            // else round down
            return new Vector3
            {
                X = (float)Math.Floor(v.X),//(float)(v.X >= 0 ? Math.Floor(v.X) : Math.Ceiling(v.X)),
                Y = (float)Math.Floor(v.Y), //(float)(v.Y >= 0 ? Math.Floor(v.Y) : Math.Ceiling(v.Y)),
                Z = (float)Math.Floor(v.Z),//(float)(v.Z >= 0 ? Math.Floor(v.Z) : Math.Ceiling(v.Z)),
            };
        }
    }
}
