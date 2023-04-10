using System;
using System.ComponentModel;
using Runtime.Grid.Data;
using Runtime.Grid.Services;
using Runtime.Terrains;
using UnityEngine;
using UnityEngine.Assertions;

namespace Runtime.Grid.Presenters
{
    [RequireComponent(typeof(Renderer))]
    public class TerrainVariantHumbleObject : MonoBehaviour
    {
        public sealed class Controller : IDisposable
        {
            private readonly ITerrainVariantRenderer _terrainVariantRenderer;
            private readonly Renderer _renderer;
            private readonly IAddressableManager _addressableManager;
            private readonly IGridCellViewModel _viewModel;

            public Controller(
                IGridCellViewModel viewModel,
                IAddressableManager addressableManager,
                ITerrainVariantRenderer terrainVariantRenderer)
            {
                _addressableManager = addressableManager ?? throw new ArgumentNullException(nameof(addressableManager));
                
                _viewModel = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
                
                _terrainVariantRenderer = terrainVariantRenderer ??
                                          throw new ArgumentNullException(nameof(terrainVariantRenderer));
            }

            public void Initialize()
            {
                _viewModel.PropertyChanged += CellOnPropertyChanged;
                SetMainTexture(_viewModel.TerrainType);
            }

            public void Dispose()
            {
                _viewModel.PropertyChanged -= CellOnPropertyChanged;
            }

            private void CellOnPropertyChanged(object sender, PropertyChangedEventArgs e)
            {
                var cell = (IGridCellViewModel)sender;

                switch (e.PropertyName)
                {
                    case nameof(IGridCellViewModel.IsPinned):
                    case nameof(IGridCellViewModel.IsHighlighted):
                    {
                        _terrainVariantRenderer.SetIsHighlighted(cell.IsHighlighted);
                        return;
                    }
                    case nameof(IGridCellViewModel.IsSelected):
                    {
                        _terrainVariantRenderer.SetIsSelected(cell.IsSelected);
                        return;
                    }
                    case nameof(IGridCellViewModel.TerrainType):
                    {
                        SetMainTexture(cell.TerrainType);
                        return;
                    }
                }
            }

            private void SetMainTexture(TerrainType terrainType)
            {
                var terrainVariant = _addressableManager.GetTerrainVariantByType(terrainType);
                _terrainVariantRenderer.SetMainTexture(terrainVariant.TextureOverride);
            }
        }

        [SerializeField] private GridCellFacade facade;

        private Controller _controller;
        private IAddressableManager _addressableManager;
        private Renderer _rendererComponent;

        private void Awake()
        {
            Assert.IsNotNull(facade, "facade != null");

            _addressableManager = ServiceInjector.Instance.AddressableManager;
            Assert.IsNotNull(_addressableManager, "_addressableManager != null");

            _rendererComponent = GetComponent<Renderer>();
            Assert.IsNotNull(_rendererComponent, "_renderer != null");
        }

        private void Start()
        {
            Assert.IsNotNull(facade.ViewModel, "facade.ViewModel != null");

            _controller = new Controller(facade.ViewModel, _addressableManager,
                new TerrainVariantRenderer(_rendererComponent));
            _controller.Initialize();
        }

        private void OnDestroy()
        {
            _controller?.Dispose();
        }
    }
}