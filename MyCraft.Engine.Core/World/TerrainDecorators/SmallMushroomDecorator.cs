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
    public class SmallMushroomDecorator : AbstractTerrainDecorator
    {
        public static float Rate = 0.015f;
        private static Random _random = new Random();

        public SmallMushroomDecorator()
        {
        }

        public override void Build(BlockMap blockMap, Vector3 pos)
        {
            if (_random.NextDouble() < Rate)
            {
                if (blockMap.BlockMatches(pos, b => (b?.GetType() == typeof(GrassBlock))))
                {
                    blockMap.SetBlock<SmallMushroomBlock>(pos + Vector3.UnitY, replace: false);
                }
            }
        }
    }
}
