using System.Threading;
using Cysharp.Threading.Tasks;
using Runtime.Grid.Presenters;
using Runtime.Terrains;
using UnityEngine.AddressableAssets;

namespace Runtime.Grid.Services
{
    public interface IAddressableManager
    {
        UniTask ClearDependencyCacheAsync(CancellationToken token = default);
        UniTask DownloadDependenciesAsync(CancellationToken token = default);
        UniTask LoadSceneAsync(AssetReference world, CancellationToken token);
        ITerrainVariant[] GetTerrainVariants();
        GridCellPresenter GetCellPrefab();

        ITerrainVariant GetTerrainVariantByType(TerrainType terrainType);

    }
}