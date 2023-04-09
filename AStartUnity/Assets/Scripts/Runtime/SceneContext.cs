using System.Linq;
using Runtime.Grid.Data;
using Runtime.Grid.Services;

namespace Runtime
{
    public sealed class SceneContext
    {
        public int RowCount { get; set; }
        public int ColCount { get; set; }
        public GridCellSave[] Cells { get; set; }

        public bool HasCells()
        {
            return Cells != null && Cells.Any();
        }
    }
}