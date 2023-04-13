using System;
using System.Linq;
using Runtime.Grid.Models;

namespace Runtime
{
    public static class TerrainTypeHelpers
    {
        private static readonly Random Random = new();

        public static bool IsWalkable(this TerrainType source)
        {
            switch (source)
            {
                case TerrainType.Grass:
                case TerrainType.Forest:
                case TerrainType.Desert:
                case TerrainType.Mountain:
                    return true;
                case TerrainType.Water:
                    return false;
                case TerrainType.Unknown:
                default:
                    throw new ArgumentOutOfRangeException(nameof(source), source, null);
            }
        }
        
        public static int GetDaysToTravelCost(this TerrainType source){
            switch (source)
            {
                case TerrainType.Grass:
                    return 1;
                case TerrainType.Forest:
                    return 2;
                case TerrainType.Desert:
                    return 5;
                case TerrainType.Mountain:
                    return 10;
                case TerrainType.Water:
                    return int.MaxValue;
                case TerrainType.Unknown:
                default:
                    throw new ArgumentOutOfRangeException(nameof(source), source, null);
            }
        }
        
        static TerrainTypeHelpers()
        {
            Types = Enum.GetValues(typeof(TerrainType)).OfType<TerrainType>().Where(x => x != TerrainType.Unknown)
                .ToArray();
        }

        private static TerrainType[] Types { get; }

        public static TerrainType GetRandomTerrainType()
        {
            return Types[Random.Next(0, Types.Length)];
        }
    }
}