using MyCraft.Engine.World;
using MyCraft.Util;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCraft.Engine.Core.World.Biomes
{
    public class BiomeGenerator
    {
        private IBiome _defaultBiome;

        private FastNoise _heatNoise;
        private FastNoise _humidityNoise;
        private List<BiomeLookupInfo> _biomes;

        // TODO refactor lookup into a seperate class?
        public BiomeGenerator(MountainBiome mountainBiome, WoodlandBiome woodlandBiome, SandBiome sandBiome, MushroomBiome mushroomBiome)
        {
            var r = new Random();

            _heatNoise = new FastNoise(r.Next());
            _heatNoise.SetNoiseType(FastNoise.NoiseType.Cellular);
            _heatNoise.SetFrequency(0.0014f);
            _heatNoise.SetCellularDistanceFunction(FastNoise.CellularDistanceFunction.Natural);
            _heatNoise.SetCellularReturnType(FastNoise.CellularReturnType.CellValue);
            _heatNoise.SetCellularJitter(0.6f);

            _humidityNoise = new FastNoise(r.Next());
            _humidityNoise.SetNoiseType(FastNoise.NoiseType.Cellular);
            _humidityNoise.SetFrequency(0.0014f);
            _humidityNoise.SetCellularDistanceFunction(FastNoise.CellularDistanceFunction.Natural);
            _humidityNoise.SetCellularReturnType(FastNoise.CellularReturnType.CellValue);
            _humidityNoise.SetCellularJitter(0.6f);

            _biomes = new List<BiomeLookupInfo>
            {
                //new BiomeLookupInfo(0.5f, 1f, 0, 0.5f, woodlandBiome),
                //new BiomeLookupInfo(0, 0.25f, 0, 0.5f, mountainBiome),
                new BiomeLookupInfo(0.5f, 2f, 0, 0.5f, sandBiome),
                new BiomeLookupInfo(0.5f, 2f, 0.5f, 2f, mushroomBiome),
            };

            _defaultBiome = woodlandBiome;
        }

        public IBiome SampleBiome(int x, int z)
        {
            var heat = _heatNoise.GetCellular(x, z) + 0.5f;
            var humidity = _humidityNoise.GetCellular(x, z) + 0.5f;

            var biome = _biomes
                .Where(b => heat <= b.MaxTemp && heat >= b.MinTemp)
                .Where(b => humidity <= b.MaxHumidity && humidity >= b.MinHumidity)
                .FirstOrDefault()?.Biome;

            return biome ?? _defaultBiome;
        }
    }

    [DebuggerDisplay("{MinTemp}, {MaxTemp}, {MinHumidity}, {MaxHumidity}, {Biome}")]
    internal class BiomeLookupInfo
    {
        public BiomeLookupInfo(float minTemp, float maxTemp, float minHumidity, float maxHumidity, IBiome biome)
        {
            MinTemp = minTemp;
            MaxTemp = maxTemp;
            MinHumidity = minHumidity;
            MaxHumidity = maxHumidity;
            Biome = biome;
        }

        public float MinTemp { get; }

        public float MaxTemp { get; }

        public float MinHumidity { get; }

        public float MaxHumidity { get; }

        public IBiome Biome { get; }
    }
}
