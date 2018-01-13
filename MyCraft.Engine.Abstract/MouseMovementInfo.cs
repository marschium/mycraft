using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCraft.Engine.Abstract
{
    public class MouseMovementInfo
    {
        public double HorizontalDelta { get; set; }

        public double VerticalDelta { get; set; }

        public bool Clicked { get; set; }

        /// <summary>
        /// Positive is up, negative is down.
        /// </summary>
        public float WheelMovement { get; set; }
    }
}
