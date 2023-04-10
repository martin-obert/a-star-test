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

        public void InstantiateGridCell(IGridCellViewModel viewModel)
        {
            var prefab = _addressableManager.GetCellPrefab();
            Object.Instantiate(prefab).SetViewModel(viewModel);
        }
    }
}