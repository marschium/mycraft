using MyCraft.Engine.Core.Blocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCraft.Engine.Core.Util
{
    public interface IBlockSetup
    {
        void RunSetup(Block block);
    }
}
