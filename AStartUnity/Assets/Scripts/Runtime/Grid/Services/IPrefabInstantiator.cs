using Runtime.Grid.Data;

namespace Runtime.Grid.Services
{
    public interface IPrefabInstantiator
    {
        void InstantiateGridCell(IGridCellViewModel viewModel);
    }
}