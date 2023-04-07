using Runtime.Terrains;

namespace Runtime.Grid.Services
{
    public sealed class GridCellSave
    {
        public int ColIndex { get; set; }
        public int RowIndex { get; set; }
        public TerrainType TerrainType { get; set; }
    }
}