using System.Collections.Generic;
using System.Linq;
using Runtime.Grid.Data;
using Runtime.Terrains;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceLocations;

namespace Runtime.Grid.Presenters
{
    [CreateAssetMenu(menuName = "Grid/Cell Repository", fileName = "Grid Cell Repository", order = 0)]
    public sealed class GridCellRepository : ScriptableObject, IGridCellRepository, ITerrainVariantRepository
    {
        private const string PrefabVariants = "grid_cell_variant";
        private readonly List<GridCellPresenter> _prefabs = new();

        public GridCellPresenter GetPrefab(ITerrainVariant terrainVariant, Transform parent)
        {
            CheckPrefabsLoaded();

            var presenterPrefab = _prefabs.FirstOrDefault(x => x.TerrainVariant == terrainVariant);
            return Instantiate(presenterPrefab, parent);
        }

        private void CheckPrefabsLoaded()
        {
            if (_prefabs.Any()) return;

            var handle =
                Addressables.LoadAssetsAsync<GameObject>(new List<string> { PrefabVariants },
                    presenter => _prefabs.Add(presenter.GetComponent<GridCellPresenter>()),
                    Addressables.MergeMode.Union, false);
            handle.WaitForCompletion();
        }

        public ITerrainVariant GetRandomTerrainVariant()
        {
            CheckPrefabsLoaded();
            var range = Random.Range(0, _prefabs.Count);
            return _prefabs[range].TerrainVariant;
        }
    }
}