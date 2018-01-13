using MyCraft.Engine.Abstract;

using MyCraft.Engine.Core.Blocks;
using MyCraft.Engine.Core.Util;
using MyCraft.Engine.World.Events;
using MyCraft.Util;
using OpenTK;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCraft.Engine.World
{
    public class BlockMap
    {
        private ChunkBuilder _chunkBuilder;
        private readonly IUpdateScheduler _updateScheduler;
        private readonly Func<Type, Block> _blockFactory;

        public BlockMap(IUpdateScheduler updateScheduler, Func<Type, Block> blockFactory, ChunkBuilder chunkBuilder)
        {
            Chunks = new ConcurrentDictionary<Vector3, Chunk>();
            _chunkBuilder = chunkBuilder;
            _updateScheduler = updateScheduler;
            _blockFactory = blockFactory;
        }

        public ConcurrentDictionary<Vector3, Chunk> Chunks { get; private set; }

        public Chunk LoadChunkWithTerrain(Vector3 pos)
        {
            Chunk chunk = null;
            var floored = WorldUtil.FloorToChunkPosition(pos);
            if (Chunks.ContainsKey(floored))
            {
                chunk = Chunks[floored];
                if (!chunk.HasBlocksGenerated)
                {
                    _chunkBuilder.PopulateExisting(this, chunk);

                }
            }
            else
            {
                chunk = new Chunk(floored, _updateScheduler);
                AddChunk(chunk);
                _chunkBuilder.PopulateExisting(this, chunk);
            }
            return chunk;
        }

        /// <summary>
        /// Get chunk at position c. C is in world space
        /// </summary>
        public Chunk GetChunk(Vector3 c, bool loadIfMissing)
        {
            Chunk chunk = null;
            var floored = WorldUtil.FloorToChunkPosition(c);
            if (Chunks.ContainsKey(floored))
            {
                chunk = Chunks[floored];
            }
            return chunk;

        }

        public Block GetBlock(Vector3 v)
        {
            // TODO what if Y is bigger than chunk size?
            var pos = WorldUtil.FloorToChunkPosition(WorldUtil.ToBlockPosition(v));
            Chunk c = GetChunk(pos, false);
            if (c != null)
            {
                var positionInChunk = new Vector3(v.X - pos.X, v.Y, v.Z - pos.Z);
                return c[(int)positionInChunk.X, (int)positionInChunk.Y, (int)positionInChunk.Z];
            }
            return null;
        }

        public void RemoveBlock(Vector3 v)
        {
            SetBlock(v, null);
        }

        /// <summary>
        /// Mass set blocks. All block updates are treated as replace
        /// </summary>
        public void SetBlocks(IEnumerable<(Vector3, Block)> updates)
        {
            // group by chunks
            // iterate trough chunks
            // set the blocks TODO might need mass update function on chunks
            foreach (var g in updates.GroupBy(vb => GetChunk(vb.Item1, false)))
            {
                g.Key.SuppressChangeFlag = true;
                foreach(var vb in g)
                {
                    var blockPos = WorldUtil.ToBlockPosition(vb.Item1);
                    SetBlockInChunk(blockPos, vb.Item2, g.Key, null); // TODO block generation until all items are done or add mass-set to chunk
                }
                g.Key.SuppressChangeFlag = false;
            }
        }

        public bool SetBlock<T>(Vector3 v, bool replace = true, IBlockSetup blockSetup = null)
            where T : Block
        {
            var block = _blockFactory(typeof(T));
            block.Position = v;
            return SetBlock(v, block, replace, blockSetup);
        }

        public bool SetBlock(Vector3 v, Block block, bool replace = true, IBlockSetup blockSetup = null)
        {
            var blockPos = WorldUtil.ToBlockPosition(v);
            Chunk chunk = GetChunk(blockPos, false);
            // TODO if the chunk is null (not loaded yet) queue update for later on
            if (chunk != null)
            {
                SetBlockInChunk(blockPos, block, chunk, blockSetup, replace);
            }
            return true;
        }

        public void AddChunk(Chunk chunk)
        {
            Chunks[chunk.Position] = chunk;
        }

        public int GetHeight(int x, int z)
        {
            return GetHeight(new Vector3(x, 0, z));
        }

        /// <summary>
        /// Returns -1 if the coord does not exist
        /// </summary>
        public int GetHeight(Vector3 v)
        {
            if (Chunks.ContainsKey(v))
            {
                return Chunks[v].GetNonTransparentHeight((int)Math.Floor(v.X), (int)Math.Floor(v.Z));
            }
            else
            {
                return 0;
            }
        }

        public bool BlockMatches(Vector3 v, Func<Block, bool> predicate)
        {
            var block = GetBlock(v);
            return predicate(block);
        }

        public bool BlockIsEmpty(Vector3 v)
        {
            var block = GetBlock(v);
            if (block == null)
            {
                return true;
            }
            return false;
        }

        public bool BlockIsEmptyOrTransparent(Vector3 v)
        {
            var block = GetBlock(v);
            if (block == null)
            {
                return true;
            }
            return block.IsTransparent;
        }

        private void SetBlockInChunk(Vector3 blockPos, Block block, Chunk chunk, IBlockSetup blockSetup, bool replace = true)
        {
            var chunkPos = WorldUtil.FloorToChunkPosition(blockPos);
            var positionInChunk = new Vector3(blockPos.X - chunkPos.X, blockPos.Y, blockPos.Z - chunkPos.Z);// TODO chunk should probably handle world coords -> chunk coords
            var existingBlock = chunk[(int)positionInChunk.X, (int)positionInChunk.Y, (int)positionInChunk.Z];
            if (!replace && existingBlock != null)
            {
                return;
            }

            // run OnRemove if there is a block about to be replaced
            var existingUpdateBlock = existingBlock as IUpdateBlock;
            existingUpdateBlock?.OnRemove();

            chunk[(int)positionInChunk.X, (int)positionInChunk.Y, (int)positionInChunk.Z] = block;
            blockSetup?.RunSetup(block); // run setup

            // update any blocks nearby
            //UpdateBlock(blockPos + Vector3.UnitZ);
            //UpdateBlock(blockPos - Vector3.UnitZ);
            //UpdateBlock(blockPos + Vector3.UnitX);
            //UpdateBlock(blockPos - Vector3.UnitX);

            // if the block was on the edge of a chunk, rengen the neighbouring chunk
            Chunk neighbour = null;
            if ((int)positionInChunk.X == 0)
            {
                Chunks.TryGetValue(chunkPos - new Vector3(Chunk.Width, 0, 0), out neighbour);
            }
            else if ((int)positionInChunk.X == Chunk.Width - 1)
            {
                Chunks.TryGetValue(chunkPos + new Vector3(Chunk.Width, 0, 0), out neighbour);
            }
            if (neighbour != null)
            {
                neighbour.HasChanged = true;
            }

            if ((int)positionInChunk.Z == 0)
            {
                Chunks.TryGetValue(chunkPos - new Vector3(0, 0, Chunk.Width), out neighbour);
            }
            else if ((int)positionInChunk.Z == Chunk.Width - 1)
            {
                Chunks.TryGetValue(chunkPos + new Vector3(0, 0, Chunk.Width), out neighbour);
            }
            if (neighbour != null)
            {
                neighbour.HasChanged = true;
            }
        }

        private void UpdateBlock(Vector3 position)
        {
            var block = GetBlock(position);
            var updateBlock = block as IUpdateBlock;
            if (updateBlock != null)
            {
                updateBlock.Update();
            }
        }

        private void CheckNeighbouringChunk(Chunk originalChunk, Vector3 neighbourPosition)
        {
            var neighbour = GetChunk(neighbourPosition, false);
            if (neighbour != originalChunk)
            {
                neighbour.HasChanged = true;
            }
        }
    }
}
