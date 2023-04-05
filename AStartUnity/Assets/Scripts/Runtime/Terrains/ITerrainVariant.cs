namespace Runtime.Terrains
{
    public interface ITerrainVariant
    {
        int DaysTravelCost { get; }
        
        bool IsWalkable { get; }
    }
}