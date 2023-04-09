using System.Linq;
using Runtime.Grid.Data;

namespace Runtime
{
    public sealed class SceneContext
    {
        public int RowCount { get; set; }
        public int ColCount { get; set; }
        public IGridCellViewModel[] Cells { get; set; }

        public bool HasCells()
        {
            return Cells != null && Cells.Any();
        }
    }
}