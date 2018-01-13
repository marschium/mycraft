using MyCraft.Engine.Abstract;
using MyCraft.Engine.GameObjects;
using MyCraft.Engine.World.TerrainDecorator;
using MyCraft.Engine.World.TerrainDecorators;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCraft.Engine.World
{
    public class ChunkBuilder
    {
        private readonly IUpdateScheduler _scheduler;
        private TerrainGenerator _terrainGenerator;
        private Random _random;

        public ChunkBuilder(TerrainGenerator terrainGenerator, IUpdateScheduler scheduler)
        {
            _terrainGenerator = terrainGenerator;
            _random = new Random();
            _scheduler = scheduler;
        }


        public Chunk BuildChunk(BlockMap blockMap, Vector3 position)
        {
            var chunk = new Chunk(position, _scheduler);
            PopulateExisting(blockMap, chunk);
            return chunk;
        }

        public void PopulateExisting(BlockMap blockMap, Chunk chunk)
        {
            chunk.HasBlocksGenerated = true;

            // generate the terrain
            _terrainGenerator.GenerateChunkTerrain(blockMap, chunk);

            // spawn any mobs
            if (_random.NextDouble() > 0.9d)
            {

                // TEST SLIME
                //var h = chunk.GetHeight(16, 16);
                //var slimePosition = new Vector3(chunk.Position.X + 16, h, chunk.Position.Z + 16);
                //var slime = GameObject.Create<GreenSlime>(slimePosition);
            }
        }
    }
}
