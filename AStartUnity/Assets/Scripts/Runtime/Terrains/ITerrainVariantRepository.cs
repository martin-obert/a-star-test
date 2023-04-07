using Cysharp.Threading.Tasks;

namespace Runtime.Terrains
{
    public interface ITerrainVariantRepository
    {
        ITerrainVariant GetRandomTerrainVariant();
        ITerrainVariant GetTerrainVariant(TerrainType argTerrainType);
        UniTask InitAsync();
    }
}