using System;
using NUnit.Framework;
using Runtime.Grid.Presenters;
using Runtime.Grid.Services;
using Runtime.Terrains;

namespace Tests
{
    public class DataValidationTests
    {
        [Test]
        public void GridCellDataModel_Pass()
        {
            var data = new GridCellSave
            {
                ColIndex = 0,
                RowIndex = 0,
                TerrainType = TerrainType.Grass
            };

            ThrowHelpers.ValidateGridCellDataOrThrow(data);
            Assert.Pass();
        }

        [TestCase(0, -1, TerrainType.Grass)]
        public void GridCellDataModel_InvalidRowIndex(int colIndex, int rowIndex, TerrainType type)
        {
            var data = new GridCellSave
            {
                ColIndex = colIndex,
                RowIndex = rowIndex,
                TerrainType = type
            };


            Assert.Throws(Is.TypeOf<ArgumentOutOfRangeException>().And.Message.StartsWith("RowIndex"),
                () => ThrowHelpers.ValidateGridCellDataOrThrow(data));
        }
        
        [TestCase(-1, 0, TerrainType.Grass)]
        public void GridCellDataModel_InvalidColIndex(int colIndex, int rowIndex, TerrainType type)
        {
            var data = new GridCellSave
            {
                ColIndex = colIndex,
                RowIndex = rowIndex,
                TerrainType = type
            };

            Assert.Throws(Is.TypeOf<ArgumentOutOfRangeException>().And.Message.StartsWith("ColIndex"),
                () => ThrowHelpers.ValidateGridCellDataOrThrow(data));
        }
        
        
        [TestCase(0, 0, TerrainType.Unknown)]
        public void GridCellDataModel_InvalidTerrainType(int colIndex, int rowIndex, TerrainType type)
        {
            var data = new GridCellSave
            {
                ColIndex = colIndex,
                RowIndex = rowIndex,
                TerrainType = type
            };

            Assert.Throws(Is.TypeOf<ArgumentOutOfRangeException>().And.Message.Contains("terrain"),
                () => ThrowHelpers.ValidateGridCellDataOrThrow(data));
        }
    }
}