using System.Linq;
using Runtime.Grid.Data;

namespace Runtime
{
    public sealed class SceneContext
    {
        public int RowCount { get; set; }
        public int ColCount { get; set; }
        public IGridCell[] Cells { get; set; }

        public bool IsValid()
        {
            return RowCount > 0 && ColCount > 0 && Cells != null && Cells.Any();
        }
    }
}