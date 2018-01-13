using System;
using System.Collections.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using MyCraft.Engine.Core.Blocks;

namespace MyCraft.Engine.World.TerrainDecorator
{
    public class DeadPlantDecorator : AbstractTerrainDecorator
    {
        public static float Rate = 0.001f;
        private static Random _random = new Random();

        public DeadPlantDecorator()
        {
        }

        public override void Build(BlockMap blockMap, Vector3 pos)
        {
            if (_random.NextDouble() < Rate)
            {
                if (blockMap.BlockMatches(pos, b => (b?.GetType() == typeof(SandBlock))))
                {
                    blockMap.SetBlock<DeadPlantBlock>(pos + Vector3.UnitY, replace: false);
                }
            }
        }
    }
}
