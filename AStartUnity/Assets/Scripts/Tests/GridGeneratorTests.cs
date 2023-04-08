using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Runtime.Grid;
using Runtime.Grid.Data;
using Runtime.Grid.Presenters;
using Runtime.Grid.Services;
using Runtime.Terrains;
using UniRx;
using UnityEngine;
using Object = UnityEngine.Object;

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

        [Test]
        public void GridService_GenerateGrid_Pass()
        {
            var presenter = new GameObject().AddComponent<MeshRenderer>().gameObject.AddComponent<GridCellPresenter>();

            var prefabInstantiatorMock = new Mock<IPrefabInstantiator>();

            prefabInstantiatorMock.Setup(x => x.InstantiateGridCellPresenter(It.IsAny<IGridCell>()))
                .Returns<IGridCell>((_) => presenter);

            var gridCellMock = new Mock<IGridCell>();
            var gridService = new GridService(prefabInstantiatorMock.Object);

            gridService.InstantiateGrid(1, 1, new[] { gridCellMock.Object });

            Assert.That(gridService.Cells, Has.Length.EqualTo(1));
            prefabInstantiatorMock.Verify(x => x.InstantiateGridCellPresenter(It.IsAny<IGridCell>()), Times.Once);
        }

        [Test]
        public void GridService_GenerateGrid_ArgumentFail()
        {
            var presenter = new GameObject().AddComponent<MeshRenderer>().gameObject.AddComponent<GridCellPresenter>();

            var prefabInstantiatorMock = new Mock<IPrefabInstantiator>();

            prefabInstantiatorMock.Setup(x => x.InstantiateGridCellPresenter(It.IsAny<IGridCell>()))
                .Returns<IGridCell>((_) => presenter);

            var gridCellMock = new Mock<IGridCell>();

            var gridService = new GridService(prefabInstantiatorMock.Object);

            Assert.Catch<ArgumentOutOfRangeException>(() =>
                gridService.InstantiateGrid(1, 1, null));
            Assert.Catch<ArgumentOutOfRangeException>(() =>
                gridService.InstantiateGrid(0, 0, new[] { gridCellMock.Object }));
            Assert.Catch<ArgumentOutOfRangeException>(() =>
                gridService.InstantiateGrid(-1, -1, new[] { gridCellMock.Object }));
        }
    }
}