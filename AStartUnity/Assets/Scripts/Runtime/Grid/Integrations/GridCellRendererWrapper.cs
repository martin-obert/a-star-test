using System;
using System.ComponentModel;
using Runtime.DependencyInjection;
using Runtime.Grid.Models;
using Runtime.Grid.Services;
using Runtime.Services;
using UnityEngine;
using UnityEngine.Assertions;

namespace Runtime.Grid.Integrations
{
    /// <summary>
    /// Humble object that wraps around <see cref="IGridCellRenderer"/>, integrating <see cref="IGridCellViewModel"/>
    /// </summary>
    [RequireComponent(typeof(Renderer))]
    public class GridCellRendererWrapper : MonoBehaviour
    {
        /// <summary>
        /// Controller contains the main logic of this component allowing better testing
        /// </summary>
        public sealed class Controller : IDisposable
        {
            private readonly IGridCellRenderer _gridCellRenderer;
            private readonly IAddressableManager _addressableManager;
            private readonly IGridCellViewModel _viewModel;
            
            public Controller(
                IGridCellViewModel viewModel,
                IAddressableManager addressableManager,
                IGridCellRenderer gridCellRenderer)
            {
                _addressableManager = addressableManager ?? throw new ArgumentNullException(nameof(addressableManager));
                
                _viewModel = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
                
                _gridCellRenderer = gridCellRenderer ??
                                          throw new ArgumentNullException(nameof(gridCellRenderer));
            }

            public void Initialize()
            {
                _viewModel.PropertyChanged += CellOnPropertyChanged;
                SetMainTexture(_viewModel.TerrainType);
                _gridCellRenderer.SetIsWalkable(_viewModel.IsWalkable);
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
                        var cellIsHighlighted = cell.IsHighlighted || cell.IsPinned;
                        _gridCellRenderer.SetIsHighlighted(cellIsHighlighted);
                        return;
                    }
                    case nameof(IGridCellViewModel.IsSelected):
                    {
                        _gridCellRenderer.SetIsSelected(cell.IsSelected);
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
                _gridCellRenderer.SetMainTexture(terrainVariant.TextureOverride);
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
                new GridCellRenderer(_rendererComponent));
            
            _controller.Initialize();
        }

        private void OnDestroy()
        {
            _controller?.Dispose();
        }
    }
}