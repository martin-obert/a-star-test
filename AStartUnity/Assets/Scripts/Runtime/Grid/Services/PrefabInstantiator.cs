using Runtime.Grid.Data;
using Runtime.Grid.Presenters;
using UnityEngine;

namespace Runtime.Grid.Services
{
    internal class PrefabInstantiator : IPrefabInstantiator
    {
        private readonly IAddressableManager _addressableManager;
        public GridCellPresenter InstantiateGridCellPresenter(IGridCell gridCell)
        {
            var prefab = _addressableManager.GetCellPrefab();
            var terrainVariant = _addressableManager.GetTerrainVariantByType(gridCell.TerrainType);
            prefab.SetDataModel(gridCell, terrainVariant);
            return Object.Instantiate(prefab);
        }
    }
}