using MyCraft.Engine.Abstract;
using MyCraft.Engine.Core.Blocks;
using MyCraft.Engine.World;
using MyCraft.Engine.World.TerrainDecorator;
using MyCraft.Util;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCraft.Engine.Core.World.Biomes
{
    public class WoodlandBiome : IBiome
    {
        private FastNoise _heightNoise;

        private static int nonce = Math.Min(32, Chunk.Height / 2);

        // Biome determines the following
        //  Top level blocks
        //  Mid level blocks
        //  Low level blocks
        //  Heightmap
        //  Trees
        //  Flower
        //  Other decoration

        public WoodlandBiome(IEnumerable<AbstractTerrainDecorator> decorators)
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

        public Block TopLevelBlock => new GrassBlock();

        public Block UnderwaterBlock => new DirtBlock();

        public Block MidLevelBlock => new DirtBlock();

        public Block BottomLevelBlock => new StoneBlock();

        public IEnumerable<AbstractTerrainDecorator> Decorators { get;}

        public int GetHeight(int x, int z)
        {
            return Math.Max((int)((_heightNoise.GetPerlinFractal(x, z)  + 0.5f) * nonce), 1);
        }
    }
}
