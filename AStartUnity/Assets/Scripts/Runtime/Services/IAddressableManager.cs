using System.Threading;
using Cysharp.Threading.Tasks;
using Runtime.Grid.Integrations;
using Runtime.Grid.Models;
using UnityEngine.AddressableAssets;

namespace Runtime.Services
{
    public interface IAddressableManager
    {
        UniTask ClearDependencyCacheAsync(CancellationToken token = default);
        UniTask DownloadDependenciesAsync(CancellationToken token = default);
        UniTask LoadSceneAsync(AssetReference world, CancellationToken token);
        ITerrainVariant[] GetTerrainVariants();
        GridCellFacade GetCellPrefab();

        ITerrainVariant GetTerrainVariantByType(TerrainType terrainType);

    }
}