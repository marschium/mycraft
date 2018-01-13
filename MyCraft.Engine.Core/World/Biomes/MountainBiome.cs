using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyCraft.Engine.Core.Blocks;
using MyCraft.Engine.World;
using MyCraft.Engine.World.TerrainDecorator;
using MyCraft.Util;
using OpenTK;

namespace MyCraft.Engine.Core.World.Biomes
{
    public class MountainBiome : IBiome
    {
        private static int MaxTerrainHeight = Chunk.Height - 16;
        private FastNoise _heightNoise;

        public MountainBiome(IEnumerable<AbstractTerrainDecorator> decorators)
        {
            Decorators = decorators;
            _heightNoise = new FastNoise(new Random().Next());
            _heightNoise.SetNoiseType(FastNoise.NoiseType.PerlinFractal);
            _heightNoise.SetFrequency(0.01f);
            _heightNoise.SetInterp(FastNoise.Interp.Quintic);
            _heightNoise.SetFractalType(FastNoise.FractalType.FBM);
            _heightNoise.SetFractalOctaves(2);
            _heightNoise.SetFractalLacunarity(2);
            _heightNoise.SetFractalGain(0.1f);
        }


        public Block BottomLevelBlock => new StoneBlock();

        public IEnumerable<AbstractTerrainDecorator> Decorators { get; }

        public Block MidLevelBlock => new StoneBlock();

        public Block TopLevelBlock => new StoneBlock();

        public Block UnderwaterBlock => new DirtBlock();

        public int GetHeight(int x, int z)
        {

            return Math.Max((int)((_heightNoise.GetPerlinFractal(x, z) + 0.5f) * MaxTerrainHeight), 1);
        }
    }
}
