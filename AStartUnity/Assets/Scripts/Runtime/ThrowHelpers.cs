using System;
using Runtime.Grid.Models;

namespace Runtime
{
    public static class ThrowHelpers
    {
        public static void ValidateGridCellDataOrThrow(GridCellDataModel value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            if (value.RowIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(value.RowIndex), value.RowIndex,
                    $"{nameof(value.RowIndex)} must be greater or equal to 0");
            if (value.ColIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(value.ColIndex), value.ColIndex,
                    $"{nameof(value.ColIndex)} must be greater or equal to 0");
            
            if(value.TerrainType == TerrainType.Unknown)
                throw new ArgumentOutOfRangeException(nameof(value.TerrainType), value.TerrainType,
                    "Unsupported terrain type");
        }
    }
}