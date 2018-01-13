using MyCraft.Engine.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCraft.Engine.Core.Blocks
{
    /// <summary>
    /// A block that updates itself and modifies the world
    /// </summary>
    public interface IUpdateBlock
    {
        void ScheduleUpdate();

        void Update();

        void OnRemove();
    }
}
