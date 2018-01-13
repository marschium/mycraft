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
    public class WatchtowerDecorator : AbstractTerrainDecorator
    {
        private static Random _random = new Random();

        public float Rate => 0.00005f;

        public override void Build(BlockMap blockMap, Vector3 pos)
        {
            if (_random.NextDouble() > Rate || (blockMap.BlockMatches(pos, b => !b?.GetType().Equals(typeof(GrassBlock)) ?? true)))
            {
                return;
            }

            var h = 15;
            BuildFloor(pos + new Vector3(-2, 0, -2), blockMap, 5);
            BuildWall(pos + new Vector3(-3, 0, -2), blockMap, 5, h, 0, 1);
            BuildWall(pos + new Vector3(3, 0, -2), blockMap, 5, h, 0, 1);
            BuildWall(pos + new Vector3(-2, 0, 3), blockMap, 5, h, 1, 0);
            BuildWall(pos + new Vector3(-2, 0, -3), blockMap, 5, h, 1, 0);
            BuildFloor(pos + new Vector3(-2, h - 2, -2), blockMap, 5);

            blockMap.SetBlock(pos + new Vector3(-3, h, -2), new StoneWallBlock());
            blockMap.SetBlock(pos + new Vector3(3, h, -2), new StoneWallBlock());
            blockMap.SetBlock(pos + new Vector3(3, h, 2), new StoneWallBlock());
            blockMap.SetBlock(pos + new Vector3(-3, h, 2), new StoneWallBlock());
            blockMap.SetBlock(pos + new Vector3(2, h, 3), new StoneWallBlock());
            blockMap.SetBlock(pos + new Vector3(2, h, -3), new StoneWallBlock());
            blockMap.SetBlock(pos + new Vector3(-2, h, 3), new StoneWallBlock());
            blockMap.SetBlock(pos + new Vector3(-2, h, -3), new StoneWallBlock());

            blockMap.SetBlock(pos + new Vector3(-3, h, 0), new StoneWallBlock());
            blockMap.SetBlock(pos + new Vector3(3, h, 0), new StoneWallBlock());
            blockMap.SetBlock(pos + new Vector3(0, h, 3), new StoneWallBlock());
            blockMap.SetBlock(pos + new Vector3(0, h, -3), new StoneWallBlock());
        }

        private void BuildFloor(Vector3 pos, BlockMap blockMap, int w)
        {
            for(var x= 0; x < w; x++)
            {
                for (var z = 0; z < w; z++)
                {
                    blockMap.SetBlock(pos + new Vector3(x, 0, z), new WoodPlanksBlock());
                }
            }
        }

        private void BuildWall(Vector3 startPos, BlockMap blockMap, int len, int height, int xd, int zd)
        {
            var pos = startPos;
            var startHeight = pos.Y;
            for (var l = 0; l < len; l++)
            {
                for (var h = 0; h < height; h++)
                {
                    blockMap.SetBlock(pos + new Vector3(0, h, 0), new StoneWallBlock());
                }
                pos += new Vector3(xd, blockMap.GetHeight(pos), zd);
            }
        }
    }
}
