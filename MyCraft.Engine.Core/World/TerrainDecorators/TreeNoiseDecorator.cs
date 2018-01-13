using MyCraft.Engine.World.TerrainDecorator;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCraft.Engine.World.TerrainDecorators
{
    public class TreeNoiseDecorator : TreeDecorator
    {

        private Random _random;
        private float _maxSpawnChance = 0.05f;

        public TreeNoiseDecorator()
        {
            //_random = new Random();
            //var perlin = new ImprovedPerlin();
            //perlin.Seed = _random.Next();

            //var billow = new Billow();
            //billow.Frequency = 0.5f;
            //billow.OctaveCount = 4;
            //billow.Primitive3D = perlin;

            //_noiseOutput = new Abs(billow);
            //_noiseMap = new NoiseMap(Chunk.Width, Chunk.Width);
            //_noiseMapBuilder = new NoiseMapBuilderPlane
            //{
            //    SourceModule = _noiseOutput,
            //    NoiseMap = _noiseMap
            //};
            //_noiseMapBuilder.SetSize(Chunk.Width, Chunk.Width);
        }

        // TODO more effectient way of iterating the chunk. only do it once for all decorators
        //public override void Build(Chunk chunk)
        //{
        //    // loop each coord in the chunk.
        //    // samaple the noise map
        //    // if random > value from noise map, plant a tree

        //    _noiseMapBuilder.SetBounds((chunk.Position.X / Chunk.Width), (chunk.Position.X / Chunk.Width) + 1, (chunk.Position.Z / Chunk.Width), (chunk.Position.Z / Chunk.Width) + 1);
        //    _noiseMapBuilder.Build();

        //    for (int z = 0; z < Chunk.Width; z++)
        //    {
        //        for(int x = 0; x < Chunk.Width; x++)
        //        {
        //            var reading = _noiseMap.GetValue(x, z) * _maxSpawnChance;
        //            if (_random.NextDouble() < reading)
        //            {
        //                var pos = chunk.Position + new Vector3(x, chunk.GetHeight(x, z), z);

        //                if (World.Current.BlockMatches(pos, b => !b?.IsFluid ?? true))
        //                {
        //                    BuildTree(pos);
        //                }
        //            }
        //        }
        //    }
        //}
    }
}
