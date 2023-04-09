using Runtime.Grid.Data;
using Runtime.Grid.Presenters;

namespace Runtime.Grid.Services
{
    public interface IPrefabInstantiator
    {
        IGridCellPresenterController InstantiateGridCellPresenter(IGridCellViewModel viewModel);
    }
}