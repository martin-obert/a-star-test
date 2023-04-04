namespace Runtime.Terrains
{
    public interface ITerrainVariantRepository
    {
        ITerrainVariant GetRandomTerrainVariant(int row, int col);
    }
}