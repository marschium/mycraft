using MyCraft.Engine.Blocks;
using MyCraft.Engine.Terrain;
using MyCraft.Util;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCraft.Engine
{
    public class Physics
    {
        public static float Gravity = 4.6f;
                
        public static RayCastHit RayCast(Vector3 pos, Vector3 ray, float distance)
        {
            //length of ray from current position to next x or y-side
            double sideDistX;
            double sideDistY;
            double sideDistZ;

            float deltaXDist = Math.Abs(1f / ray.X); // distance ray has to travel to cover one unit of X
            float deltaYDist = Math.Abs(1f / ray.Y); // distance ray has to travel to cover one unit of Y
            float deltaZDist = Math.Abs(1f / ray.Z); // distance ray has to travel to cover one unit of Z

            //what direction to step in
            int stepX;
            int stepY;
            int stepZ;

            var mapPos = pos.Floor();

            //calculate step and initial sideDist
            if (ray.X < 0)
            {
                stepX = -1;
                sideDistX = (pos.X - mapPos.X) * deltaXDist;
            }
            else
            {
                stepX = 1;
                sideDistX = (mapPos.X + 1.0 - pos.X) * deltaXDist;
            }
            if (ray.Y < 0)
            {
                stepY = -1;
                sideDistY = (pos.Y - mapPos.Y) * deltaYDist;
            }
            else
            {
                stepY = 1;
                sideDistY = (mapPos.Y + 1.0 - pos.Y) * deltaYDist;
            }
            if (ray.Z < 0)
            {
                stepZ = -1;
                sideDistZ = (pos.Z - mapPos.Z) * deltaZDist;
            }
            else
            {
                stepZ = 1;
                sideDistZ = (mapPos.Z + 1.0 - pos.Z) * deltaZDist;
            }

            var hitSide = BlockFaceHit.Front;
            Block block = null;
            //perform DDA
            while (block == null && (Math.Abs(sideDistX + sideDistY + sideDistZ) < distance * 4))
            {
                //jump to next map square
                if (sideDistX < sideDistZ)
                {
                    if (sideDistX < sideDistY)
                    {
                        sideDistX += deltaXDist;
                        mapPos.X += stepX;
                        hitSide = stepX > 0 ? BlockFaceHit.Left : BlockFaceHit.Right;
                    }
                    else
                    {
                        sideDistY += deltaYDist;
                        mapPos.Y += stepY;
                        hitSide = stepY > 0 ? BlockFaceHit.Bottom : BlockFaceHit.Top;
                    }
                }
                else
                {
                    if (sideDistZ < sideDistY)
                    {
                        sideDistZ += deltaZDist;
                        mapPos.Z += stepZ;
                        hitSide = stepZ > 0 ? BlockFaceHit.Back : BlockFaceHit.Front;
                    }
                    else
                    {
                        sideDistY += deltaYDist;
                        mapPos.Y += stepY;
                        hitSide = stepY > 0 ? BlockFaceHit.Bottom : BlockFaceHit.Top;
                    }
                }
                //Check if ray has hit a wall
                block = World.Current.GetBlock(mapPos);
            }

            if (block != null)
            {
                return new RayCastHit
                {
                    Block = block,
                    Location = mapPos,
                    Face = hitSide
                };
            }
            else
            {
                return null;
            }
        }
    }

    public enum Side
    {
        X,
        Y,
        Z
    }

    public enum BlockFaceHit
    {
        Front,
        Back,
        Right,
        Left,
        Top,
        Bottom
    }

    public class RayCastHit
    {
        /// <summary>
        /// Block that was hit.
        /// </summary>
        public Block Block { get; set; }

        /// <summary>
        /// Block position of raycast hit.
        /// </summary>
        public Vector3 Location { get; set; }

        public BlockFaceHit Face { get; set; }
    }
}
