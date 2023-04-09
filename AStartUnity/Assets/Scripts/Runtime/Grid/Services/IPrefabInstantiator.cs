using Runtime.Grid.Data;

namespace Runtime.Grid.Services
{
    public interface IPrefabInstantiator
    {
        IGridCellViewModel InstantiateGridCellPresenter(GridCellSave viewModel);
    }
}