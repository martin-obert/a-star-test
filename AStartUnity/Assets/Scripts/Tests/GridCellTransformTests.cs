using System;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using Runtime.Grid.Data;
using Runtime.Grid.Presenters;
using Runtime.Grid.Services;
using Runtime.Terrains;
using UnityEngine;
using Assert = UnityEngine.Assertions.Assert;

namespace Tests
{
    public class GridCellTransformTests
    {
        [SetUp]
        public void SetupTests()
        {
        }

        public static IEnumerable<TestCaseData> Controller_PassCaseSource()
        {
            var terrainVariantMock = new Mock<ITerrainVariant>();
            terrainVariantMock.SetupGet(x => x.IsWalkable).Returns(true);
            terrainVariantMock.SetupGet(x => x.Type).Returns(TerrainType.Grass);

            var configurationMock = new Mock<GridCellTransformWrapper.Configuration>();
            configurationMock.SetupGet(x => x.LiftAmount).Returns(1);

            return new[]
            {
                new TestCaseData(terrainVariantMock.Object, configurationMock.Object)
            };
        }
        public static IEnumerable<TestCaseData> Controller_NonWalkableTerrainCaseSource()
        {
            var terrainVariantMock = new Mock<ITerrainVariant>();
            terrainVariantMock.SetupGet(x => x.IsWalkable).Returns(false);
            terrainVariantMock.SetupGet(x => x.Type).Returns(TerrainType.Grass);

            var configurationMock = new Mock<GridCellTransformWrapper.Configuration>();
            configurationMock.SetupGet(x => x.LiftAmount).Returns(1);

            return new[]
            {
                new TestCaseData(terrainVariantMock.Object, configurationMock.Object)
            };
        }

        [TestCaseSource(nameof(Controller_PassCaseSource))]
        public void Controller_IsHighlighted_Pass(
            ITerrainVariant terrainVariant,
            GridCellTransformWrapper.Configuration configuration)
        {
            var position = Vector3.zero;


            var transformMock = new Mock<IGridCellTransform>();
            transformMock.SetupSet<Vector3>(x => x.Position = It.IsAny<Vector3>()).Callback(x => position = x);
            transformMock.SetupGet(x => x.Position).Returns(position);


            IGridCellViewModel viewModelMock =
                new GridCellViewModel(new GridCellSave { TerrainType = TerrainType.Mountain }, terrainVariant);

            var controller = new GridCellTransformWrapper.Controller(viewModelMock, transformMock.Object,
                configuration);

            controller.Initialize();

            viewModelMock.ToggleHighlighted(true);

            controller.Update(1);
            transformMock.Verify(x => x.Position, Times.AtLeastOnce);
            AssertIsLifted(position, configuration.LiftAmount);

            viewModelMock.ToggleHighlighted(false);
            controller.Update(1);
            AssertHasLanded(position);

            controller.Dispose();
        }


        [TestCaseSource(nameof(Controller_PassCaseSource))]
        public void Controller_IsSelected_Pass(
            ITerrainVariant terrainVariant,
            GridCellTransformWrapper.Configuration configuration)
        {
            var position = Vector3.zero;


            var transformMock = new Mock<IGridCellTransform>();
            transformMock.SetupSet<Vector3>(x => x.Position = It.IsAny<Vector3>()).Callback(x => position = x);
            transformMock.SetupGet(x => x.Position).Returns(position);


            IGridCellViewModel viewModelMock =
                new GridCellViewModel(new GridCellSave{TerrainType = TerrainType.Grass}, terrainVariant);

            var controller = new GridCellTransformWrapper.Controller(viewModelMock, transformMock.Object,
                configuration);

            controller.Initialize();

            viewModelMock.ToggleSelected(true);
            controller.Update(1);
            AssertIsLifted(position, configuration.LiftAmount);

            viewModelMock.ToggleSelected(false);
            controller.Update(1);
            AssertHasLanded(position);

            controller.Dispose();
        }

        [TestCaseSource(nameof(Controller_PassCaseSource))]
        public void Controller_IsPinned_Pass(
            ITerrainVariant terrainVariant,
            GridCellTransformWrapper.Configuration configuration)
        {
            var position = Vector3.zero;


            var transformMock = new Mock<IGridCellTransform>();
            transformMock.SetupSet<Vector3>(x => x.Position = It.IsAny<Vector3>()).Callback(x => position = x);
            transformMock.SetupGet(x => x.Position).Returns(position);


            IGridCellViewModel viewModelMock =
                new GridCellViewModel(new GridCellSave { TerrainType = TerrainType.Mountain }, terrainVariant);

            var controller = new GridCellTransformWrapper.Controller(viewModelMock, transformMock.Object,
                configuration);

            controller.Initialize();

            viewModelMock.TogglePinned(true);
            controller.Update(1);
            AssertIsLifted(position, configuration.LiftAmount);

            viewModelMock.TogglePinned(false);
            controller.Update(1);
            AssertHasLanded(position);

            controller.Dispose();
        }

        [TestCaseSource(nameof(Controller_PassCaseSource))]
        public void Controller_Disposed_Pass(
            ITerrainVariant terrainVariant,
            GridCellTransformWrapper.Configuration configuration)
        {
            var position = Vector3.zero;


            var transformMock = new Mock<IGridCellTransform>();
            transformMock.SetupSet<Vector3>(x => x.Position = It.IsAny<Vector3>()).Callback(x => position = x);
            transformMock.SetupGet(x => x.Position).Returns(position);


            IGridCellViewModel viewModelMock =
                new GridCellViewModel(new GridCellSave{TerrainType = TerrainType.Grass}, terrainVariant);

            var controller = new GridCellTransformWrapper.Controller(viewModelMock, transformMock.Object,
                configuration);

            controller.Initialize();

            viewModelMock.TogglePinned(true);
            controller.Update(1);
            AssertIsLifted(position, configuration.LiftAmount);

            viewModelMock.TogglePinned(false);
            controller.Update(1);
            AssertHasLanded(position);

            controller.Dispose();
            
            viewModelMock.TogglePinned(true);
            controller.Update(1);
            AssertHasLanded(position);
        }

        [TestCaseSource(nameof(Controller_NonWalkableTerrainCaseSource))]
        public void NonWalkableTerrain_Pass(
            ITerrainVariant terrainVariant,
            GridCellTransformWrapper.Configuration configuration)
        {
            var position = Vector3.zero;


            var transformMock = new Mock<IGridCellTransform>();
            transformMock.SetupSet<Vector3>(x => x.Position = It.IsAny<Vector3>()).Callback(x => position = x);
            transformMock.SetupGet(x => x.Position).Returns(position);


            IGridCellViewModel viewModelMock =
                new GridCellViewModel(new GridCellSave { TerrainType = TerrainType.Grass }, terrainVariant);

            var controller = new GridCellTransformWrapper.Controller(viewModelMock, transformMock.Object,
                configuration);

            controller.Initialize();

            viewModelMock.TogglePinned(true);
            controller.Update(1);
            AssertHasLanded(position);

            viewModelMock.TogglePinned(false);
            controller.Update(1);
            AssertHasLanded(position);

            viewModelMock.ToggleHighlighted(true);
            controller.Update(1);
            AssertHasLanded(position);

            viewModelMock.ToggleHighlighted(false);
            controller.Update(1);
            AssertHasLanded(position);

            viewModelMock.ToggleSelected(true);
            controller.Update(1);
            AssertHasLanded(position);

            viewModelMock.ToggleSelected(false);
            controller.Update(1);
            AssertHasLanded(position);

            controller.Dispose();
            
            viewModelMock.TogglePinned(true);
            controller.Update(1);
            AssertHasLanded(position);
        }

        private static void AssertIsLifted(Vector3 position, float liftAmount)
        {
            Assert.IsTrue(Math.Abs(position.y - liftAmount) <= 0);
        }

        private static void AssertHasLanded(Vector3 position)
        {
            Assert.IsTrue(position.y == 0);
        }
    }
}