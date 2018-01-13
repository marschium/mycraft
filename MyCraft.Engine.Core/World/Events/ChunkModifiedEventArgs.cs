using MyCraft.Engine.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCraft.Engine.World.Events
{
    public class ChunkModifiedEventArgs : EventArgs
    {
        public ChunkModifiedEventArgs(Chunk chunk)
        {
            Chunk = chunk;
        }

        public Chunk Chunk { get; }
    }
}
