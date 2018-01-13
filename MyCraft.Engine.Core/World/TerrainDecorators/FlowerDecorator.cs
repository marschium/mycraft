using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using MyCraft.Engine.Core.Blocks;

namespace MyCraft.Engine.World.TerrainDecorator
{
    public class FlowerDecorator : AbstractTerrainDecorator
    {
        public static float Rate = 0.03f;
        private static Random _random = new Random();

        public FlowerDecorator()
        {
        }

        public override void Build(BlockMap blockMap, Vector3 pos)
        {
            if (_random.NextDouble() < Rate)
            {
                if (blockMap.BlockMatches(pos, b => (b?.GetType() == typeof(GrassBlock))))
                {
                    blockMap.SetBlock<FlowerBlock>(pos + Vector3.UnitY, replace: false);
                }
            }
        }
    }
}
