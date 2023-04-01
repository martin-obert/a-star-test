using Runtime.Grid.Data;

namespace Runtime.Grid.Services
{
    public interface IGridManager
    {
        IGridCell[] CurrentCells { get; }

        void GenerateGrid();
    }
}