using System;
using NUnit.Framework;
using Runtime;
using Runtime.Grid;
using Runtime.Grid.Models;
using Runtime.Grid.Services;

namespace Tests
{
    public class DataValidationTests
    {
        [Test]
        public void GridCellDataModel_Pass()
        {
            var data = new GridCellDataModel
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
            var data = new GridCellDataModel
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
            var data = new GridCellDataModel
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
            var data = new GridCellDataModel
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