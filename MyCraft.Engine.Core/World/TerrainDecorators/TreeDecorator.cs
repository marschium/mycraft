using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using MyCraft.Engine.Core.Blocks;

namespace MyCraft.Engine.World.TerrainDecorator
{
    public class TreeDecorator : AbstractTerrainDecorator
    {
        private static Random _random = new Random();

        public static int MinHeight = 5;
        public static int MaxHeight = 10;
        public static float Rate = 0.01f;

        protected HashSet<Vector3> _placedBlocks;

        public TreeDecorator()
        {
        }

        public override void Build(BlockMap blockMap, Vector3 pos)
        {
            if (_random.NextDouble() < Rate)
            {
                BuildTree(blockMap, pos);
            }
        }

        protected void BuildTree(BlockMap blockMap, Vector3 position)
        {
            if (blockMap.BlockMatches(position, b => b?.GetType().Equals(typeof(GrassBlock)) ?? false))
            {
                var h = _random.Next(MinHeight, MaxHeight);
                for (int y = 0; y < h; y++)
                {
                    blockMap.SetBlock<WoodBlock>(position + (Vector3.UnitY * (y + 1)));
                    if (y >= h - 3)
                    {
                        for (int x = -1; x <= 1; x++)
                        {
                            for (int z = -1; z <= 1; z++)
                            {
                                if (x != 0 || z != 0)
                                {
                                    blockMap.SetBlock<LeafBlock>(position + new Vector3(x, y + 1, z));
                                }
                            }
                        }
                    }
                }
            }           
        }
    }
}
