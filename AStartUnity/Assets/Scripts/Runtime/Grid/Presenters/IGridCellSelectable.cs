
namespace Runtime.Grid.Presenters
{
    public interface IGridCellSelectable : IGridCellHoverable
    {
        bool IsSelected { get;  }
        void ToggleSelection(bool? value = null);
    }
}