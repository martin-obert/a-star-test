using System.Linq;
using NUnit.Framework;
using Runtime.Grid;

namespace Tests
{
    public class GridGeneratorTests
    {
        /// <summary>
        /// Most basic test to check generated result
        /// </summary>
        [Test]
        public void GenerateCells_MinimalPass()
        {
            var grid = GridGenerator.GenerateGrid(1, 2);
            Assert.That(grid, Is.Not.Null);
            Assert.That(grid, Has.Length.EqualTo(2), "Should only generate 2 cells");
            Assert.That(grid.Last().ColIndex, Is.EqualTo(1), "Last col index of a cell should be 1");
            Assert.That(grid.Last().RowIndex, Is.EqualTo(0),"Last row index of a cell should be 0");
        }

    }
}
