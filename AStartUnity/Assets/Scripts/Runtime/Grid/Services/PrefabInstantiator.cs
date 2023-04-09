using System;
using Runtime.Grid.Data;
using Object = UnityEngine.Object;

namespace Runtime.Grid.Services
{
    internal class PrefabInstantiator : IPrefabInstantiator
    {
        private readonly IAddressableManager _addressableManager;

        public PrefabInstantiator(IAddressableManager addressableManager)
        {
            _addressableManager = addressableManager ?? throw new ArgumentNullException(nameof(addressableManager));
        }

        public IGridCellViewModel InstantiateGridCellPresenter(GridCellSave save)
        {
            var prefab = _addressableManager.GetCellPrefab();
            var terrainVariant = _addressableManager.GetTerrainVariantByType(save.TerrainType);
            return Object.Instantiate(prefab)
                .BindDataModel(save, terrainVariant);
        }
    }
}