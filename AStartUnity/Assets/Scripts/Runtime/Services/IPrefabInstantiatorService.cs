using Runtime.Grid.Models;

namespace Runtime.Services
{
    public interface IPrefabInstantiatorService
    {
        void InstantiateGridCell(IGridCellViewModel viewModel);
    }
}