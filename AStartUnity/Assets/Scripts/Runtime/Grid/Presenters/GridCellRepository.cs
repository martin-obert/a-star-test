using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Runtime.Terrains;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Random = UnityEngine.Random;

namespace Runtime.Grid.Presenters
{
    [CreateAssetMenu(menuName = "Grid/Cell Repository", fileName = "Grid Cell Repository", order = 0)]
    public sealed class GridCellRepository : ScriptableObject, IGridCellRepository, ITerrainVariantRepository, IDisposable
    {
        private const string PrefabVariants = "grid_cell_variant";
        private readonly List<GridCellPresenter> _prefabs = new();
        private bool _isInitializing;
        private bool _isInitialized;
        public IEnumerator Init()
        {
            if (_isInitialized || _isInitializing) yield break;


            if (_prefabs.Any()) yield break;
            _isInitializing = true;

            var handle =
                Addressables.LoadAssetsAsync<GameObject>(new List<string> { PrefabVariants },
                    presenter => _prefabs.Add(presenter.GetComponent<GridCellPresenter>()),
                    Addressables.MergeMode.Union, false);
            yield return handle;

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
            var range = Random.Range(0, _prefabs.Count);
            return _prefabs[range].TerrainVariant;
        }

        private void ThrowIfInitializingOrNotInitialized()
        {
            if (!_isInitialized) throw new Exception($"{name} - not yet initialized: call {nameof(Init)}() method and wait for result");
            if (_isInitializing) throw new Exception($"{name} - is still initializing wait for result");
        }

        private void OnDisable()
        {
            Dispose();
        }

        public void Dispose()
        {
            _isInitialized = false;
            _isInitializing = false;
        }
    }
}