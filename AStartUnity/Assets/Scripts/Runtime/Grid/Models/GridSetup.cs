using System.Linq;

namespace Runtime.Grid.Models
{
    public sealed class GridSetup
    {
        public int RowCount { get; set; }
        public int ColCount { get; set; }
        public GridCellDataModel[] Cells { get; set; }

        public bool HasCells()
        {
            return Cells != null && Cells.Any();
        }
    }
}