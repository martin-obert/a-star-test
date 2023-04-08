using Runtime.Grid.Data;
using Runtime.Grid.Presenters;

namespace Runtime.Grid.Services
{
    public interface IPrefabInstantiator
    {
        GridCellPresenter InstantiateGridCellPresenter(IGridCell cell);
    }
}