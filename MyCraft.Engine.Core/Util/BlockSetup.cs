using MyCraft.Engine.Core.Blocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCraft.Engine.Core.Util
{
    public class BlockSetup<TBlock> : IBlockSetup
        where TBlock : Block
    {
        private ICollection<Action<TBlock>> _setupCommands;

        public BlockSetup()
        {
            _setupCommands = new List<Action<TBlock>>();
        }

        public void RunSetup(Block block)
        {
            foreach(var action in _setupCommands)
            {
                action.Invoke(block as TBlock);
            }
        }

        public BlockSetup<TBlock> With(Action<TBlock> action)
        {
            _setupCommands.Add(action);
            return this;
        }
    }
}
