using System;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Runtime.Definitions;
using Runtime.Grid.Integrations;
using Runtime.Grid.Models;
using Runtime.Grid.Services;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

namespace Runtime.Services
{
    internal class AddressableManager : IAddressableManager, IDisposable
    {
        private readonly GameDefinitions _gameDefinitions;
        private readonly ITerrainVariant[] _terrainVariants;
        private AsyncOperationHandle<GameObject> _handle;
        private GridCellFacade _cell;

        public AddressableManager(GameDefinitions gameDefinitions, ITerrainVariant[] terrainVariants)
        {
            _gameDefinitions = gameDefinitions;
            _terrainVariants = terrainVariants;
        }

        public UniTask ClearDependencyCacheAsync(CancellationToken token = default)
        {
            return Addressables.ClearDependencyCacheAsync(_gameDefinitions.PreloadLabel, true)
                .WithCancellation(token);
        }

        public async UniTask DownloadDependenciesAsync(CancellationToken token = default)
        {
            var downloadableSize = await GetDownloadSizeAsync(token);
            if (downloadableSize > 0)
                await Addressables.DownloadDependenciesAsync(_gameDefinitions.PreloadLabel);

            _handle = _gameDefinitions.HexTile.LoadAssetAsync<GameObject>();
            var result = await _handle.WithCancellation(token);
            _cell = result.GetComponent<GridCellFacade>();
        }

        public UniTask<long> GetDownloadSizeAsync(CancellationToken token = default)
        {
            return Addressables.GetDownloadSizeAsync(_gameDefinitions.PreloadLabel)
                .WithCancellation(token);
        }

        public async UniTask LoadSceneAsync(AssetReference world, CancellationToken token)
        {
            await Addressables.LoadSceneAsync(world, LoadSceneMode.Additive).WithCancellation(token);
        }

        public ITerrainVariant[] GetTerrainVariants()
        {
            return _terrainVariants;
        }

        public GridCellFacade GetCellPrefab()
        {
            return _cell;
        }

        public ITerrainVariant GetTerrainVariantByType(TerrainType terrainType)
        {
            var source = GetTerrainVariants();
            if (source == null) throw new ArgumentNullException(nameof(source));
            return source.FirstOrDefault(x => x.Type == terrainType);
        }

        public void Dispose()
        {
            if (_handle.IsValid())
                Addressables.Release(_handle);
        }
    }
}