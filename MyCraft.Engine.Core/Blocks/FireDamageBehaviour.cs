using MyCraft.Engine.Core.GameObjects;
using MyCraft.Engine.GameObjects;
using MyCraft.Engine.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCraft.Engine.Core.Blocks
{
    /// <summary>
    /// Given a block that has been damaged by fire. Changes the block and/or world based on the block state
    /// </summary>
    public class FireDamageBehaviour
    {
        private BlockMap _blockMap;
        private GameObjectInitaliser _gameObjectInitaliser;

        public FireDamageBehaviour(BlockMap blockMap, GameObjectInitaliser gameObjectInitaliser)
        {
            _blockMap = blockMap;
            _gameObjectInitaliser = gameObjectInitaliser;
        }

        public void FireDamageBlock(Block block)
        {
            // anything other than grass, remove the block
            switch (block)
            {
                case ExplosiveBlock explosiveBlock:
                    _gameObjectInitaliser.Create<Explosion>(block.Position);
                    break;
                case GrassBlock grassBlock:
                    _blockMap.SetBlock<BurntGrassBlock>(block.Position);
                    break;
                default:
                    _blockMap.RemoveBlock(block.Position);
                    break;
            }
        }
    }
}
