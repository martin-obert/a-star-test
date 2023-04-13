using System;
using Runtime.Grid.Models;
using Object = UnityEngine.Object;

namespace Runtime.Services
{
    internal class PrefabInstantiatorService : IPrefabInstantiatorService
    {
        private readonly IAddressableManager _addressableManager;

        public PrefabInstantiatorService(IAddressableManager addressableManager)
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