﻿using Moq;
using NUnit.Framework;
using Runtime.Grid.Presenters;
using Runtime.Grid.Services;
using Runtime.Terrains;
using UnityEngine;

namespace Tests
{
    public class TerrainRendererTests
    {
        private Mock<IAddressableManager> _addressableManagerMock;

        [SetUp]
        public void SetupTest()
        {
            _addressableManagerMock = new Mock<IAddressableManager>();
            var terrainVariantMock = new Mock<ITerrainVariant>();
            _addressableManagerMock.Setup(x => x.GetTerrainVariantByType(It.IsAny<TerrainType>()))
                .Returns(() => terrainVariantMock.Object);
        }
        
        [Test]
        public void TerrainVariantController_Pass()
        {
            var viewModelMock = new GridCellViewModel(new GridCellSave(), new Mock<ITerrainVariant>().Object);
            var terrainVariantRendererMock = new Mock<ITerrainVariantRenderer>();

            var controller = new TerrainVariantHumbleObject.Controller(
                viewModelMock,
                _addressableManagerMock.Object,
                terrainVariantRendererMock.Object);

            controller.Initialize();
            
            terrainVariantRendererMock.Verify(x =>
                x.SetMainTexture(It.IsAny<Texture>()), Times.Once);
            
            viewModelMock.ToggleHighlighted(true);
            viewModelMock.TogglePinned(true);

            terrainVariantRendererMock.Verify(x =>
                x.SetIsHighlighted(It.Is<bool>(v => v == true)), Times.Exactly(2));

            viewModelMock.ToggleSelected(true);

            terrainVariantRendererMock.Verify(x =>
                x.SetIsSelected(It.Is<bool>(v => v == true)), Times.Once);
        }
    }
}