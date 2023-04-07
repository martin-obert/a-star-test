namespace Runtime.Terrains
{
    public enum TerrainType
    {
        Unknown = 0,
        Grass = 1,
        Forest = 2,
        Desert = 3,
        Mountain = 4,
        Water = 5
    }

    public interface ITerrainVariant
    {
        int DaysTravelCost { get; }

        bool IsWalkable { get; }
        
        TerrainType Type { get; }
    }
}