using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCraft.Engine.World.TerrainDecorator
{
    public abstract class AbstractTerrainDecorator
    {
        public abstract void Build(BlockMap blockMap, Vector3 pos);
    }
}
