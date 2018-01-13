using MyCraft.Engine.Abstract;
using MyCraft.Engine.Core.Blocks;
using MyCraft.Engine.Core.World;
using MyCraft.Engine.Core.World.Biomes;
using MyCraft.Engine.World.TerrainDecorator;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCraft.Engine.World
{
    public class TerrainGenerator
    {        
        private readonly BiomeGenerator _biomeGenerator;

        public TerrainGenerator(BiomeGenerator biomeGenerator)
        {
            _biomeGenerator = biomeGenerator;
        }

        public void GenerateChunkTerrain(BlockMap blockMap, Chunk chunk)
        {
            GenerateTerrainSmoothInterpolation(blockMap, chunk);
        }

        public void PopulateTerrain(BlockMap blockMap, Chunk chunk, IBiome biome, int x, int z, int h)
        {
            bool isUnderwater = Chunk.WaterLevel > h; // TODO biome dependant?
            for (int y = 0; y < h; y++)
            {
                // don't override any blocks that have already been set by the world
                if (chunk[x, y, z] != null)
                {
                    continue;
                }

                if (y == h - 1)
                {
                    if (!isUnderwater)
                    {
                        var block = biome.TopLevelBlock;
                        block.Scheduler = chunk.Scheduler;
                        block.Position = chunk.Position + new Vector3(x, y, z);
                        chunk[x, y, z] = block;
                    }
                    else
                    {
                        var block = biome.UnderwaterBlock;
                        block.Scheduler = chunk.Scheduler;
                        block.Position = chunk.Position + new Vector3(x, y, z);
                        chunk[x, y, z] = block;
                    }
                }
                else if (y < h - 1 && y > h - 4)
                {
                    var block = biome.MidLevelBlock;
                    block.Scheduler = chunk.Scheduler;
                    block.Position = chunk.Position + new Vector3(x, y, z);
                    chunk[x, y, z] = block;
                }
                else
                {
                    var block = biome.BottomLevelBlock;
                    block.Scheduler = chunk.Scheduler;
                    block.Position = chunk.Position + new Vector3(x, y, z);
                    chunk[x, y, z] = block;
                }
            }

            if (isUnderwater)
            {
                for (int w = h; w < Chunk.WaterLevel; w++)
                {
                    WaterBlock b = new WaterBlock();
                    b.Scheduler = chunk.Scheduler;
                    b.Position = chunk.Position + new Vector3(x, w, z);
                    b.Source = b;
                    b.BlockMap = blockMap;
                    chunk[x, w, z] = b;
                }
            }

            // Add tress e.t.c.
            foreach (var decorator in biome.Decorators)
            {
                decorator.Build(blockMap, chunk.Position + new Vector3(x, h - 1, z));
            }
        }  

        public void GenerateTerrainSmoothInterpolation(BlockMap blockMap, Chunk chunk)
        {
            // break the chunk into smaller 8 x 8 chunks
            // calculate height of each corner of the small chunk
            // calculate each of heights in the small chunk by taking from biome heightmap and smoothing based on the corner values

            int SetColumn(int x, int z, Func<int, int> smoothingFunc)
            {
                var worldX = (int)chunk.Position.X + x;
                var worldZ = (int)chunk.Position.Z + z;
                var biome = _biomeGenerator.SampleBiome(worldX, worldZ);
                int h = smoothingFunc(biome.GetHeight(worldX, worldZ));

                PopulateTerrain(blockMap, chunk, biome, x, z, h);
                return h;
            }

            float Lerp(float s, float e, float t)
            {
                return s + (e - s) * t;
            }

            float Blerp(float c00, float c10, float c01, float c11, float tx, float ty)
            {
                return Lerp(Lerp(c00, c10, tx), Lerp(c01, c11, tx), ty);
            }

            void GenerateMicroChunk(int minX, int minZ, int maxX, int maxZ)
            {
                // sample the corners
                // sample the unsmoothed height for each x,z
                var c11 = SetColumn(minX, minZ, x => x);
                var c10 = SetColumn(minX, maxZ, x => x);
                var c01 = SetColumn(maxX, minZ, x => x);
                var c00 = SetColumn(maxX, maxZ, x => x);

                for(int x = minX; x <= maxX; x++)
                {
                    for(int z = minZ; z <= maxZ; z++)
                    {
                        if ((x == minX && z == minZ) ||
                            (x == minX && z == maxZ) ||
                            (x == maxX && z == minZ) ||
                            (x == maxX && z == maxZ)){
                            continue;
                        }

                        // normalise x and z respective to microchunk width and depth
                        float xNormalised = 1.0f - (float)(x - minX) / (float)(maxX - minX);
                        float zNormalised = 1.0f - (float)(z - minZ) / (float)(maxZ - minZ);

                        SetColumn(x, z, q => (int)Blerp(c00, c10, c01, c11, xNormalised, zNormalised));
                    }
                }
            }

            var microChunkSize = 8;
            for(int x = 0; x < Chunk.Width; x += microChunkSize)
            {
                for (int z = 0; z < Chunk.Width; z += microChunkSize)
                {
                    GenerateMicroChunk(x, z, x + (microChunkSize - 1), z + (microChunkSize - 1));
                }
            }
        }
    }
}
