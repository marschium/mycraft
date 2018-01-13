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
    public class CactusDecorator : AbstractTerrainDecorator
    {
        private static Random _random = new Random();

        public float Rate => 0.001f;

        public override void Build(BlockMap blockMap, Vector3 pos)
        {
            if (_random.NextDouble() > Rate || (blockMap.BlockMatches(pos, b => !b?.GetType().Equals(typeof(SandBlock)) ?? true)))
            {
                return;
            }

            for (int y  = 1; y < 4; y++)
            {
                
                var offset = new Vector3(0, y, 0);
                blockMap.SetBlock(pos + offset, new CactusBlock());
            }
        }
    }
}
