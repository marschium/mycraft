using MyCraft.Engine.World;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCraft.Engine
{
    public class ChunkLighting
    {
        public static readonly Vector3 MinShaderLightLevel = new Vector3(0.1f, 0.1f, 0.1f);

        private static int minLight = 0;
        private static int sunlight = 15;

        /// <summary>
        /// Build a lightmap for sunlight/ ambient light.
        /// </summary>
        /// <param name="chunk"></param>
        /// <returns></returns>
        public int[,,] BuildLightMap(Chunk chunk)
        {

            // TODO return a array of vec3 so that coloured lights can be used.
            var lightmap = new int[Chunk.Width, Chunk.Height, Chunk.Width];

            for (int x = 0; x < Chunk.Width; x++)
            {
                for (int z = 0; z < Chunk.Width; z++)
                {
                    //for (int y = 0; y < Chunk.Height; y++)
                    //{
                    //    lightmap[x, y, z] = 15;
                    //}

                    var pos = new Vector3(x, 0, z);

                    // TODO use world position
                    var centerHeight = chunk.GetHeight(x, z);
                    var northHeight = chunk.GetHeight(x, z + 1);
                    var northEastHeight = chunk.GetHeight(x + 1, z + 1);
                    var eastHeight = chunk.GetHeight(x + 1, z);
                    var southEastHeight = chunk.GetHeight(x + 1, z - 1);
                    var southHeight = chunk.GetHeight(x, z - 1);
                    var southWestHeight = chunk.GetHeight(x - 1, z - 1);
                    var westHeight = chunk.GetHeight(x - 1, z);
                    var northWestHeight = chunk.GetHeight(x - 1, z + 1);

                    var maxHeight = new[] { centerHeight, northHeight, northEastHeight, eastHeight, southEastHeight, southHeight, southWestHeight, westHeight, northWestHeight }.Max();

                    // interate upwards the tallest of the four neighbours at N,E,S,W
                    for (int h = chunk.GetNonTransparentHeight(x, z); h <= maxHeight; h++)
                    {
                        // populate all the top level chunks with sunlight
                        lightmap[x, h, z] = sunlight;

                        // set blocks nearby
                        // TODO maybe this loop should also change y level
                        for (int dx = -3; dx <= 3; dx++)
                        {
                            for (int dz = -3; dz <= 3; dz++)
                            {
                                if (dx == 0 && dz == 0)
                                {
                                    continue;
                                }
                                SetLight(x, h, z, dx, 0, dz, sunlight);
                            }
                        }

                    }
                }


                void SetLight(int rx, int ry, int rz, int dx, int dy, int dz, int light)
                {
                    if ((rx + dx < 0 || rx + dx >= Chunk.Width) || (ry + dy < 0 || ry + dy >= Chunk.Height) || (rz + dz < 0 || rz + dz >= Chunk.Width))
                    {
                        return;
                    }

                    var dist = Math.Abs(dx) + Math.Abs(dy) + Math.Abs(dz);
                    var blockLight = dist != 1 ? Math.Max(light - (dist + 1 * 3), minLight) : light; // reduce by 3 per block

                    if (lightmap[rx + dx, ry + dy, rz + dz] < blockLight)
                    {
                        lightmap[rx + dx, ry + dy, rz + dz] = blockLight;
                    }
                }
            }

            // TODO lighting for other light sources like torches

            return lightmap;
        }
    }
}
