using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Runtime.Terrains;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Random = System.Random;

namespace Runtime.Grid.Presenters
{
    [CreateAssetMenu(menuName = "Grid/Cell Repository", fileName = "Grid Cell Repository", order = 0)]
    public sealed class GridCellRepository : ScriptableObject, IGridCellRepository, ITerrainVariantRepository, IDisposable
    {
        private const string PrefabVariants = "grid_cell_variant";
        private readonly List<GridCellPresenter> _prefabs = new();
        private AsyncOperationHandle<IList<GameObject>> _handle;
        private bool _isInitializing;
        private bool _isInitialized;
        private readonly Random _random = new();

        public async UniTask InitAsync()
        {
            if (_isInitialized || _isInitializing) return;

            if (_prefabs.Any()) return;
            
            _isInitializing = true;

            _handle =
                Addressables.LoadAssetsAsync<GameObject>(new List<string> { PrefabVariants },
                    presenter => _prefabs.Add(presenter.GetComponent<GridCellPresenter>()),
                    Addressables.MergeMode.Union, false);
            await _handle;

            _isInitialized = true;
            _isInitializing = false;
        }

        public GridCellPresenter GetPrefab(ITerrainVariant terrainVariant, Transform parent)
        {
            ThrowIfInitializingOrNotInitialized();
            var presenterPrefab = _prefabs.FirstOrDefault(x => x.TerrainVariant == terrainVariant);
            return Instantiate(presenterPrefab, parent);
        }

        public ITerrainVariant GetRandomTerrainVariant()
        {
            ThrowIfInitializingOrNotInitialized();
            var range = _random.Next(0, _prefabs.Count);
            
            return _prefabs[range].TerrainVariant;
        }

        private void ThrowIfInitializingOrNotInitialized()
        {
            if (!_isInitialized) throw new Exception($"{name} - not yet initialized: call {nameof(InitAsync)}() method and wait for result");
            if (_isInitializing) throw new Exception($"{name} - is still initializing wait for result");
        }

        private void OnDisable()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (_isInitialized)
            {
                Addressables.Release(_handle);
            }
            _isInitialized = false;
            _isInitializing = false;
        }
    }
}