using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyCraft.Engine.Core.Blocks;
using MyCraft.Engine.Abstract;

namespace MyCraft.Engine.World
{
    public class Chunk
    {
        public static int Width= 32;
        public static int Height = 64;
        public static int WaterLevel = 8;
        
        private Block[,,] _blocks;
        private int[,] _nonTransparentHeight;
        private int[,] _height;
        private bool _hasChanged;

        public Chunk(Vector3 position, IUpdateScheduler scheduler)
        {
            Position = position;
            Scheduler = scheduler;
            _blocks = new Block[Width, Height, Width];
            _nonTransparentHeight = new int[Width, Width];   
            _height = new int[Width, Width];   
        }

        public void Clear()
        {
            _blocks = null;
        }

        public bool HasChanged {
            get
            {
                if (SuppressChangeFlag)
                {
                    return false;
                }
                return _hasChanged;
            }
            set
            {
                _hasChanged = value;
            }
        }

        public Vector3 Position { get; }
        public IUpdateScheduler Scheduler { get; }
        public ChunkModel SolidModel { get; set; }

        public ChunkModel TransparentModel { get; set; }

        public ChunkModel FloraModel { get; set; }

        public bool HasBlocksGenerated { get; set; }

        public bool IsLoaded { get; set; }

        public DateTime LastModifiedTime { get; set; }

        public bool SuppressChangeFlag { get; set; }

        public Block this[int x, int y, int z]
        {
            get
            {
                if (x < 0 || y < 0 || z < 0 || x >= Width || y >= Height || z >= Width)
                {
                    return null; // block doesn't exist here
                }
                return _blocks[x, y, z];
            }
            set
            {
                if (x < 0 || y < 0 || z < 0 || x >= Width || y >= Height || z >= Width)
                {
                    return;
                }
                LastModifiedTime = DateTime.UtcNow;
                HasChanged = true;
                _blocks[x, y, z] = value;
                if (value != null)
                {
                    if (!value.IsTransparent && y > _nonTransparentHeight[x, z])
                    {
                        _nonTransparentHeight[x, z] = y;
                    }

                    if (y > _height[x, z])
                    {
                        _height[x, z] = y;
                    }
                }
                else
                {
                    bool setTransparentHeight = false;
                    bool setHeight = false;
                    for (int d = Height - 1; d > 0; d--)
                    {

                        var b = _blocks[x, d, z];
                        if (!b?.IsTransparent ?? false)
                        {
                            _nonTransparentHeight[x, z] = d;
                            setTransparentHeight = true;
                        }
                        if (b != null)
                        {
                            _height[x, z] = d;
                            setHeight = true;
                        }
                        if (setTransparentHeight && setHeight)
                        {
                            break;
                        }
                    }
                }
            }
        }


        // TODO move to a seperate class
        // TODO choose block based on biome

        /// <summary>
        /// Get height of x,z, excluding transparent block. X and Z are relative to the chunk position
        /// </summary>
        /// <param name="x"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public int GetNonTransparentHeight(int x, int z)
        {
            if (x < 0 || z < 0 || x >= Width || z >= Width)
            {
                return -1; // block doesn't exist here
            }
            return _nonTransparentHeight[x, z];
        }
        /// <summary>
        /// Get height of x,z. X and Z are relative to the chunk position
        /// </summary>
        /// <param name="x"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public int GetHeight(int x, int z)
        {
            if (x < 0 || z < 0 || x >= Width || z >= Width)
            {
                return -1; // block doesn't exist here
            }
            return _height[x, z];
        }

        /// <summary>
        /// Enumarate the blocks with the highest Y level for each x,z
        /// </summary>
        public IEnumerable<Block> TopLevelBlocks {
            get
            {
                for(int x = 0; x < Width; x++)
                {
                    for(int z = 0; z < Width; z++)
                    {
                        var y = GetNonTransparentHeight(x, z) -1;
                        var b = _blocks[x, y, z];
                        yield return b;
                    }
                }
            }
        }

        public override bool Equals(object obj)
        {
            var b = obj as Chunk;
            if (b == null || b.Position == null)
            {
                return false;
            }
            else
            {
                return Position.Equals(b.Position);
            }
        }

        public override int GetHashCode()
        {
            return Position.GetHashCode();
        }
    }
}
