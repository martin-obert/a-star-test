using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using NUnit.Framework;
using Runtime.Grid;
using Runtime.Terrains;
using UnityEngine;

namespace Tests
{
    public class GridGeneratorTests
    {
        private sealed class TerrainVariantMock : ITerrainVariant
        {
            public int DaysTravelCost => 1;
            public bool IsWalkable { get; }
            public TerrainType Type { get; }
            public Texture TextureOverride { get; }
        }

        /// <summary>
        /// Most basic test to check generated result
        /// </summary>
        [Test]
        public void GenerateCells_MinimalPass()
        {
            var grid = GridGenerator.GenerateGrid(1, 2, () => new TerrainVariantMock());
            Assert.That(grid, Is.Not.Null);
            Assert.That(grid, Has.Length.EqualTo(2), "Should only generate 2 cells");
            Assert.That(grid.Last().ColIndex, Is.EqualTo(1), "Last col index of a cell should be 1");
            Assert.That(grid.Last().RowIndex, Is.EqualTo(0), "Last row index of a cell should be 0");
        }
    }
}