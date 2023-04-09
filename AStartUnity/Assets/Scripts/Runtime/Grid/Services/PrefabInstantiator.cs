using Runtime.Grid.Data;
using Runtime.Grid.Presenters;
using UnityEngine;

namespace Runtime.Grid.Services
{
    internal class PrefabInstantiator : IPrefabInstantiator
    {
        private readonly IAddressableManager _addressableManager;
        public IGridCellPresenterController InstantiateGridCellPresenter(IGridCellViewModel viewModel)
        {
            var prefab = _addressableManager.GetCellPrefab();
            return Object.Instantiate(prefab).InitializeController(viewModel);
        }
    }
}