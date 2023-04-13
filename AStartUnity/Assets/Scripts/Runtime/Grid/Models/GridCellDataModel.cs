namespace Runtime.Grid.Models
{
    /// <summary>
    /// POCO class, that could be easily serialized/deserialized
    /// </summary>
    public sealed class GridCellDataModel
    {
        public int ColIndex { get; set; }
        public int RowIndex { get; set; }
        public TerrainType TerrainType { get; set; }

    }
}