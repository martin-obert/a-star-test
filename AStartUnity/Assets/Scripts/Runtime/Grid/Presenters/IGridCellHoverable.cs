namespace Runtime.Grid.Presenters
{
    public interface IGridCellHoverable
    {
        bool CanHover { get; }
        void OnCursorEnter();
        void OnCursorExit();
    }
}