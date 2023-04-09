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
    public class TerrainVariantPresenter : MonoBehaviour, IDisposable
    {
        private static readonly int IsHovered = Shader.PropertyToID("_Is_Hovered");
        private static readonly int IsSelected = Shader.PropertyToID("_Is_Selected");
        private static readonly int MainTex = Shader.PropertyToID("_MainTex");
        private MaterialPropertyBlock _materialOverrides;
        
        [SerializeField]
        private GridCellPresenter presenter;
        
        private Renderer _renderer;

        private IAddressableManager _addressableManager;
        
        private void Awake()
        {
            _renderer = GetComponent<Renderer>();
            Assert.IsNotNull(_renderer, "_renderer != null");
            _materialOverrides = new MaterialPropertyBlock();
        }

        private void Start()
        {
            presenter.PropertyChanged += CellOnPropertyChanged;
            _addressableManager = ServiceInjector.Instance.AddressableManager;
            SetMainTexture(presenter.TerrainType);
        }

        private void OnDestroy()
        {
            Dispose();
        }
        private void CellOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var cell = (IGridCellViewModel)sender;

            switch (e.PropertyName)
            {
                case nameof(IGridCellViewModel.IsPinned):
                case nameof(IGridCellViewModel.IsHighlighted):
                {
                    _materialOverrides.SetFloat(IsHovered, cell.IsHighlighted || cell.IsPinned ? 1 : 0);
                    _renderer.SetPropertyBlock(_materialOverrides);
                    return;
                }
                case nameof(IGridCellViewModel.IsSelected):
                {
                    _materialOverrides.SetFloat(IsSelected, cell.IsSelected ? 1 : 0);
                    _renderer.SetPropertyBlock(_materialOverrides);
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
            _materialOverrides.SetTexture(MainTex, terrainVariant.TextureOverride);
            _renderer.SetPropertyBlock(_materialOverrides);
        }
        
        public void Dispose()
        {
            if (presenter == null) return;
            presenter.PropertyChanged -= CellOnPropertyChanged;
        }
    }
}