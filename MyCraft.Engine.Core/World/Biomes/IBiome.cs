using System;
using System.Collections.Generic;
using MyCraft.Engine.Core.Blocks;
using MyCraft.Engine.World.TerrainDecorator;

namespace MyCraft.Engine.Core.World.Biomes
{
    public interface IBiome
    {
        Block BottomLevelBlock { get; }
        IEnumerable<AbstractTerrainDecorator> Decorators { get; }
        Block MidLevelBlock { get; }
        Block TopLevelBlock { get; }
        Block UnderwaterBlock { get; }
        int GetHeight(int x, int z);
    }
}