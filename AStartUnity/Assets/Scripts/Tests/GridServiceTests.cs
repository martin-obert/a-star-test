using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using Runtime.Grid;
using Runtime.Grid.Data;
using Runtime.Grid.Services;
using Runtime.Terrains;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class GridServiceTests
    {
        private Mock<IAddressableManager> _addressableManagerMock;
        private Mock<IPrefabInstantiator> _prefabInstantiatorMock;

        [SetUp]
        public void SetupTest()
        {
            _prefabInstantiatorMock = new Mock<IPrefabInstantiator>();
            _addressableManagerMock = new Mock<IAddressableManager>();
            var terrainVariantMock = new Mock<ITerrainVariant>();
            _addressableManagerMock.Setup(x => x.GetTerrainVariantByType(It.IsAny<TerrainType>()))
                .Returns(() => terrainVariantMock.Object);

            _prefabInstantiatorMock.Setup(x =>
                x.InstantiateGridCell(It.IsAny<IGridCellViewModel>()));
        }

        /// <summary>
        /// Most basic test to check generated result
        /// </summary>
        [Test]
        public void GenerateCells_MinimalPass()
        {
            var grid = GridGenerator.GenerateGridCellDataModels(1, 2);
            Assert.That(grid, Is.Not.Null);
            Assert.That(grid, Has.Length.EqualTo(2), "Should only generate 2 cells");
            Assert.That(grid.Last().ColIndex, Is.EqualTo(1), "Last col index of a cell should be 1");
            Assert.That(grid.Last().RowIndex, Is.EqualTo(0), "Last row index of a cell should be 0");
        }

        [Test]
        public void GridService_GenerateGrid_Pass()
        {
            var gridService = new GridService(_prefabInstantiatorMock.Object, _addressableManagerMock.Object);

            gridService.InstantiateGrid(1, 1, new[] { new GridCellSave { TerrainType = TerrainType.Grass } });

            Assert.That(gridService.Cells, Has.Length.EqualTo(1));

            _addressableManagerMock.Verify(x => x.GetTerrainVariantByType(It.IsAny<TerrainType>()), Times.Once);
        }

        private static IEnumerable<TestCaseData> GridService_GenerateGrid_ArgumentFailData()
        {
            return new[]
            {
                new TestCaseData(0, 0, null),
                new TestCaseData(0, 1, null),
                new TestCaseData(1, 0, null),
                new TestCaseData(0, -1, null),
                new TestCaseData(-1, 0, null),
                new TestCaseData(-1, -1, null),
                new TestCaseData(1, 1, Array.Empty<GridCellSave>()),
                new TestCaseData(1, 1, null),
            };
        }

        [TestCaseSource(nameof(GridService_GenerateGrid_ArgumentFailData))]
        public void GenerateGrid_ArgumentFail(int rows, int cols, GridCellSave[] cells)
        {
            var gridService = new GridService(_prefabInstantiatorMock.Object, _addressableManagerMock.Object);

            Assert.Throws(
                Is.TypeOf<ArgumentOutOfRangeException>().Or.TypeOf<InvalidOperationException>().Or
                    .TypeOf<ArgumentNullException>(), () =>
                    gridService.InstantiateGrid(rows, cols, cells)
            );
        }

        [Test]
        public void UpdateHoveredCell_Pass()
        {
            var camera = new Mock<IGridRaycastCamera>();

            camera.Setup(x => x.ScreenToWorldPoint(It.IsAny<Vector2>()))
                .Returns(() => Vector3.zero);
            camera.Setup(x => x.Position)
                .Returns(() => Vector3.up);

            var gridService = new GridService(_prefabInstantiatorMock.Object, _addressableManagerMock.Object);

            gridService.InstantiateGrid(1, 1, new[] { new GridCellSave { TerrainType = TerrainType.Grass } });
            gridService.UpdateHoveringCell(camera.Object, Vector2.one);

            Assert.IsTrue(gridService.Cells.First().IsHighlighted, "IsHighlighted");
        }
    }
}