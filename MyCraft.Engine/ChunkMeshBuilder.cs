using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using MyCraft.Assets;
using MyCraft.Engine.Core.Blocks;
using System.Diagnostics;
using MyCraft.Util;
using MyCraft.Engine.World;
using System.Buffers;

namespace MyCraft.Engine
{
    public class ChunkMeshBuilder
    {
        private static Vector3 BACK = new Vector3(0, 0, -1);
        private static Vector3 FORWARD = new Vector3(0, 0, 1);
        private static Vector3 RIGHT = new Vector3(1, 0, 0);
        private static Vector3 LEFT = new Vector3(-1, 0, 0);
        private static Vector3 TOP = new Vector3(0, 1, 0);
        private static Vector3 BOTTOM = new Vector3(0, -1, 0);

        private BlockMap _blockMap;
        private TextureLoader _textureLoader;
        private ChunkLighting _chunkLighting;
        private Chunk _chunk;
        private ArrayPool<Vector3> _vec3Pool;
        private ArrayPool<Vector2> _vec2Pool;

        public ChunkMeshBuilder(BlockMap blockMap, TextureLoader textureLoader, Chunk chunk, ChunkLighting chunkLighting, ArrayPool<Vector3> vec3Pool, ArrayPool<Vector2> vec2Pool)
        {
            _blockMap = blockMap;
            _textureLoader = textureLoader;
            _chunk = chunk;
            _chunkLighting = chunkLighting;
            _vec3Pool = vec3Pool;
            _vec2Pool = vec2Pool;
        }

        /// <summary>
        /// Build the solid block mode and the transparent block model
        /// </summary>
        /// <param name="chunk">Tuple of models, first is solid blocks. Second is transparent blocks.</param>
        /// <returns></returns>
        public void BuildModels()
        {
            // TODO assign from array pool
            var bufSize = Chunk.Width * Chunk.Width * Chunk.Height;            
            Vector3[] solidVertices = _vec3Pool.Rent(bufSize);
            Vector2[] solidTextureCoords = _vec2Pool.Rent(bufSize);
            Vector3[] solidLighting = _vec3Pool.Rent(bufSize);

            Vector3[] transparentVertices = _vec3Pool.Rent(bufSize);
            Vector2[] transparentTextureCoords = _vec2Pool.Rent(bufSize);
            Vector3[] transparentLighting = _vec3Pool.Rent(bufSize);

            Vector3[] floraVertices = _vec3Pool.Rent(bufSize);
            Vector2[] floraTextureCoords = _vec2Pool.Rent(bufSize);
            Vector3[] floraLighting = _vec3Pool.Rent(bufSize);

            var sw = Stopwatch.StartNew();

            var lightMap = _chunkLighting.BuildLightMap(_chunk);

            var faceOffset = 0;
            for(int x = 0; x < Chunk.Width; x++)
            {
                for(int z = 0; z < Chunk.Width; z++)
                {
                    // TODO could this be sped up by starting at the highest Y level and stopping if all the blocks underneath are solid?
                    for(int y = 0; y < Chunk.Height; y++)
                    {
                        var chunkPosition = new Vector3(x, y, z);
                        var worldPosition = _chunk.Position + new Vector3(x, y, z);
                        // TODO generate light levels as we go and only build a map of dynamic lights?

                        var block = _chunk[x,y,z];
                        if (block == null)
                        {
                            continue;
                        }

                        var light = lightMap[x, y, z];

                        var vertices = solidVertices;
                        var textureCoords = solidTextureCoords;
                        var lighting = solidLighting;
                        var isWater = block.GetType() == typeof(WaterBlock);
                        Func<Block, bool> predicate = (Block b) => b == null || b.IsTransparent;

                        if (block.IsTransparent && !(block.GetType() == typeof(WaterBlock)))
                        {
                            vertices = floraVertices;
                            textureCoords = floraTextureCoords;
                            lighting = floraLighting;
                        }
                        else if (block.GetType() == typeof(WaterBlock))
                        {
                            vertices = transparentVertices;
                            textureCoords = transparentTextureCoords;
                            lighting = transparentLighting;
                            predicate = (Block b) => (b == null || (b.IsTransparent && (!(b.GetType() == typeof(WaterBlock)) || (((WaterBlock)b).Height < ((WaterBlock)block).Height))));
                        }
                            
                        if (!isWater && _blockMap.BlockMatches(worldPosition + BACK, predicate))
                        {
                            var tmp = ScaleBlockFace(block, block.BackFace, _blockMap.GetBlock(worldPosition + BACK)).Add(chunkPosition);
                            Array.ConstrainedCopy(tmp, 0, vertices, faceOffset, tmp.Length);
                            var tmp2 = _textureLoader.GetScaledUVCoords("atlas.png", block.BackFaceTexture, block.BackFaceUV);
                            Array.ConstrainedCopy(tmp2, 0, textureCoords, faceOffset, tmp.Length);
                            AddLightData(block.BackFace, light, lighting, faceOffset);
                            faceOffset += tmp.Length;
                        }
                        if (!isWater && _blockMap.BlockMatches(worldPosition + FORWARD, predicate))
                        {
                            var tmp = ScaleBlockFace(block, block.FrontFace, _blockMap.GetBlock(worldPosition + FORWARD)).Add(chunkPosition);
                            Array.ConstrainedCopy(tmp, 0, vertices, faceOffset, tmp.Length);
                            var tmp2 = _textureLoader.GetScaledUVCoords("atlas.png", block.FrontFaceTexture, block.FrontFaceUV);
                            Array.ConstrainedCopy(tmp2, 0, textureCoords, faceOffset, tmp.Length);
                            AddLightData(block.FrontFace, light, lighting, faceOffset);
                            faceOffset += tmp.Length;
                        }
                        if (!isWater && _blockMap.BlockMatches(worldPosition + RIGHT, predicate))
                        {
                            var tmp = ScaleBlockFace(block, block.RightFace, _blockMap.GetBlock(worldPosition + RIGHT)).Add(chunkPosition);
                            Array.ConstrainedCopy(tmp, 0, vertices, faceOffset, tmp.Length);
                            var tmp2 = _textureLoader.GetScaledUVCoords("atlas.png", block.RightFaceTexture, block.RightFaceUV);
                            Array.ConstrainedCopy(tmp2, 0, textureCoords, faceOffset, tmp.Length);
                            AddLightData(block.RightFace, light, lighting, faceOffset);
                            faceOffset += tmp.Length;
                        }
                        if (!isWater && _blockMap.BlockMatches(worldPosition + LEFT, predicate))
                        {
                            var tmp = ScaleBlockFace(block, block.LeftFace, _blockMap.GetBlock(worldPosition + LEFT)).Add(chunkPosition);
                            Array.ConstrainedCopy(tmp, 0, vertices, faceOffset, tmp.Length);
                            var tmp2 = _textureLoader.GetScaledUVCoords("atlas.png", block.LeftFaceTexture, block.LeftFaceUV);
                            Array.ConstrainedCopy(tmp2, 0, textureCoords, faceOffset, tmp.Length);
                            AddLightData(block.LeftFace, light, lighting, faceOffset);
                            faceOffset += tmp.Length;
                        }
                        if ((worldPosition + TOP).Y < Chunk.Height && _blockMap.BlockMatches(worldPosition + TOP, predicate))
                        {
                            var tmp = ScaleBlockFace(block, block.TopFace, _blockMap.GetBlock(worldPosition + TOP)).Add(chunkPosition);
                            Array.ConstrainedCopy(tmp, 0, vertices, faceOffset, tmp.Length);
                            var tmp2 = _textureLoader.GetScaledUVCoords("atlas.png", block.TopFaceTexture, block.TopFaceUV);
                            Array.ConstrainedCopy(tmp2, 0, textureCoords, faceOffset, tmp.Length);
                            AddLightData(block.TopFace, light, lighting, faceOffset);
                            faceOffset += tmp.Length;
                        }
                        if (!isWater && (worldPosition + BOTTOM).Y >= 0 && _blockMap.BlockMatches(worldPosition + BOTTOM, predicate))
                        {
                            var tmp = ScaleBlockFace(block, block.BottomFace, _blockMap.GetBlock(worldPosition + BOTTOM)).Add(chunkPosition);
                            Array.ConstrainedCopy(tmp, 0, vertices, faceOffset, tmp.Length);
                            var tmp2 = _textureLoader.GetScaledUVCoords("atlas.png", block.BottomFaceTexture, block.BottomFaceUV);
                            Array.ConstrainedCopy(tmp2, 0, textureCoords, faceOffset, tmp.Length);
                            AddLightData(block.BottomFace, light, lighting, faceOffset);
                            faceOffset += tmp.Length;
                        }
                    }
                }
            }

            sw.Stop();
            Console.WriteLine($"{nameof(ChunkMeshBuilder)}: {_chunk.Position} generated in {sw.Elapsed}");
            _chunk.SolidModel = new ChunkModel(solidVertices, solidTextureCoords, solidLighting, faceOffset);
            _chunk.TransparentModel = new ChunkModel(transparentVertices, transparentTextureCoords, transparentLighting, faceOffset);
            _chunk.FloraModel = new ChunkModel(floraVertices, floraTextureCoords, floraLighting, faceOffset);
        }

        private void AddLightData(Vector3[] vertices, int light, Vector3[] lightData, int offset)
        {
            // TODO is there a better way ot adding light data?
            var o = 0;
            foreach(var vertex in vertices)
            {
                lightData[offset + o] = new Vector3(light, light, light);
                o += 1;
            }
        }

        // Get the face vertices of a block based on the block in question and its neighouring block.
        private Vector3[] ScaleBlockFace(Block b, Vector3[] face, Block neighbour)
        {
            switch (b)
            {
                case WaterBlock waterBlock:
                    if (neighbour is WaterBlock waterBlockNeighour)
                    {
                        if (waterBlock.Height > waterBlockNeighour.Height)
                        {
                            // Only draw the upper half of the block;
                            var scaledVertices = new Vector3[face.Length];
                            face.CopyTo(scaledVertices, 0);
                            for(int i = 0; i < scaledVertices.Length; i++)
                            {
                                if (scaledVertices[i].Y == 0)
                                {
                                    scaledVertices[i] = new Vector3(scaledVertices[i].X, waterBlockNeighour.Height, scaledVertices[i].Z);
                                }
                            }
                            return scaledVertices;
                        }
                    }
                    return face;
                default:
                    return face;
            }
        }
    }
}
