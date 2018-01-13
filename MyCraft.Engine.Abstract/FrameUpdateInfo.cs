using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCraft.Engine.Abstract
{
    public class FrameUpdateInfo
    {
        public FrameUpdateInfo(double timeDelta, MouseMovementInfo mouseInfo)
        {
            TimeDelta = timeDelta;
            MouseInfo = mouseInfo;
        }

        public double TimeDelta { get; }

        public MouseMovementInfo MouseInfo { get; }
    }
}
