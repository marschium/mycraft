using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCraft.Engine.Abstract
{
    public interface IUpdateScheduler
    {
        void Schedule(TimeSpan countdown, Action action);

        Task Start();
    }
}
