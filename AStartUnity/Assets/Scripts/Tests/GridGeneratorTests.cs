using System.Linq;
using Grid;
using NUnit.Framework;

namespace Tests
{
    public class GridGeneratorTests
    {
        [Test]
        public void GridGeneratorTestsSimplePasses()
        {
            var grid = GridGenerator.GenerateGrid(1, 2);
            Assert.That(grid, Is.Not.Null);
            Assert.That(grid, Has.Length.EqualTo(2));
            Assert.That(grid.Last().GridPosition.x, Is.EqualTo(1));
            Assert.That(grid.Last().GridPosition.y, Is.EqualTo(0));
        }

    }
}
