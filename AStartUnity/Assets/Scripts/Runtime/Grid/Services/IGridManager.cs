using Runtime.Grid.Data;

namespace Runtime.Grid.Services
{
    public interface IGridManager
    {
        IGridCell HoverCell { get; }
    }
}