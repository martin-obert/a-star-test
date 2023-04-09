using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using Runtime.Grid;
using Runtime.Grid.Data;
using Runtime.Grid.Presenters;
using Runtime.Grid.Services;
using Runtime.Terrains;
using UnityEngine;

namespace Tests
{
    public class GridServiceTests
    {
        private Mock<IAddressableManager> _addressableManagerMock;
        private Mock<IPrefabInstantiator> _prefabInstantiatorMock;
        private Mock<IGridCellPresenterController> _controllerMock;

        [SetUp]
        public void SetupTest()
        {
            _prefabInstantiatorMock = new Mock<IPrefabInstantiator>();
            _addressableManagerMock = new Mock<IAddressableManager>();
            var terrainVariantMock = new Mock<ITerrainVariant>();
            _addressableManagerMock.Setup(x => x.GetTerrainVariantByType(It.IsAny<TerrainType>()))
                .Returns(() => terrainVariantMock.Object);
            _controllerMock = new Mock<IGridCellPresenterController>();
            _prefabInstantiatorMock.Setup(x =>
                x.InstantiateGridCellPresenter(It.IsAny<IGridCellViewModel>())
            ).Returns(() => _controllerMock.Object);
        }

        /// <summary>
        /// Most basic test to check generated result
        /// </summary>
        [Test]
        public void GenerateCells_MinimalPass()
        {
            var terrainVariantMock = new Mock<ITerrainVariant>();
            var grid = GridGenerator.GenerateGrid(1, 2, () => terrainVariantMock.Object);
            Assert.That(grid, Is.Not.Null);
            Assert.That(grid, Has.Length.EqualTo(2), "Should only generate 2 cells");
            Assert.That(grid.Last().ColIndex, Is.EqualTo(1), "Last col index of a cell should be 1");
            Assert.That(grid.Last().RowIndex, Is.EqualTo(0), "Last row index of a cell should be 0");
        }

        [Test]
        public void GridService_GenerateGrid_Pass()
        {
            var gridCellMock = new Mock<IGridCellViewModel>();
            var gridService = new GridService(_prefabInstantiatorMock.Object, _addressableManagerMock.Object);

            gridService.InstantiateGrid(1, 1, new[] { gridCellMock.Object });

            Assert.That(gridService.Cells, Has.Length.EqualTo(1));

            _prefabInstantiatorMock.Verify(x =>
                    x.InstantiateGridCellPresenter(It.IsAny<IGridCellViewModel>()),
                Times.Once);
            _addressableManagerMock.Verify(x => x.GetTerrainVariantByType(It.IsAny<TerrainType>()), Times.Once);
            _controllerMock.Verify(x => x.SetMainTexture(It.IsAny<Texture>()), Times.Once);
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
                new TestCaseData(1, 1, Array.Empty<IGridCellViewModel>()),
                new TestCaseData(1, 1, null),
            };
        }

        [TestCaseSource(nameof(GridService_GenerateGrid_ArgumentFailData))]
        public void GenerateGrid_ArgumentFail(int rows, int cols, IGridCellViewModel[] cells)
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
            var gridCellMock = new Mock<IGridCellViewModel>();

            camera.Setup(x => x.ScreenToWorldPoint(It.IsAny<Vector2>()))
                .Returns(() => Vector3.zero);
            camera.Setup(x => x.Position)
                .Returns(() => Vector3.up);
            
            var gridService = new GridService(_prefabInstantiatorMock.Object, _addressableManagerMock.Object);

            gridService.InstantiateGrid(1, 1, new[] { gridCellMock.Object });
            gridService.UpdateHoveringCell(camera.Object, Vector2.one);

            gridCellMock.Verify(x => x.ToggleHighlighted(true), Times.Once);
        }
    }
}