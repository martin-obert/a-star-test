using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Runtime.Grid.Presenters;
using Runtime.Terrains;
using Runtime.Ui;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;
using Random = System.Random;

namespace Runtime.Grid.Services
{
    internal class AddressableManager : IAddressableManager, IDisposable
    {
        private readonly GameDefinitions _gameDefinitions;
        private readonly ITerrainVariant[] _terrainVariants;
        private AsyncOperationHandle<GameObject> _handle;
        private GridCellPresenter _cell;
        private readonly Random _random = new();

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
            _cell = result.GetComponent<GridCellPresenter>();
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

        public ITerrainVariant GetRandomTerrainVariant()
        {
            return _terrainVariants[_random.Next(0, _terrainVariants.Length)];
        }

        public ITerrainVariant[] GetTerrainVariants()
        {
            return _terrainVariants;
        }

        public GridCellPresenter GetCellPrefab()
        {
            return _cell;
        }

        public void Dispose()
        {
            if (_handle.IsValid())
                Addressables.Release(_handle);
        }
    }
}