using MyCraft.Engine.Core.Blocks;
using MyCraft.Engine.World;
using MyCraft.Engine.World.TerrainDecorator;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCraft.Engine.Core.World.TerrainDecorators
{
    public class MushroomDecorator : AbstractTerrainDecorator
    {
        private static float Rate = 0.005f;
        private static Random _random = new Random();

        public MushroomDecorator()
        {
        }

        public override void Build(BlockMap blockMap, Vector3 pos)
        {
            if (_random.NextDouble() < Rate)
            {
                BuildMushrrom(blockMap, pos);
            }
        }

        private void BuildMushrrom(BlockMap blockMap, Vector3 pos)
        {
            if (!blockMap.BlockMatches(pos, b => b?.GetType().Equals(typeof(GrassBlock)) ?? false))
            {
                return;
            }
            var h = _random.Next(3, 7);
            for (int y = 0; y < h; y++)
            {
                blockMap.SetBlock<MushroomTrunkBlock>(pos + (Vector3.UnitY * (y + 1)));
                if (y == h - 2)
                {
                    for (int x = -2; x <= 2; x+= 4)
                    {
                        for (int z = -2; z <= 2; z++)
                        {
                            blockMap.SetBlock<MushroomBlock>(pos + new Vector3(x, y + 1, z));
                        }
                    }
                    for (int z = -2; z <= 2; z += 4)
                    {
                        for (int x = -2; x <= 2; x++)
                        {
                            blockMap.SetBlock<MushroomBlock>(pos + new Vector3(x, y + 1, z));
                        }
                    }
                }
                else if (y == h - 1)
                {
                    for (int x = -2; x <= 2; x++)
                    {
                        for (int z = -2; z <= 2; z++)
                        {
                            if (x != 0 || z != 0)
                            {
                                blockMap.SetBlock<MushroomBlock>(pos + new Vector3(x, y + 1, z));
                            }
                        }
                    }
                }
            }
        }
    }
}
