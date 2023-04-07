using System.ComponentModel;
using Runtime.Grid.Data;
using Runtime.Terrains;
using UnityEngine;
using UnityEngine.Assertions;

namespace Runtime.Grid.Presenters
{
    [RequireComponent(typeof(Renderer))]
    public sealed class GridCellPresenter : MonoBehaviour
    {
        private IGridCell _cell;
        private MaterialPropertyBlock _materialOverrides;
        private Renderer _renderer;
        private static readonly int IsHovered = Shader.PropertyToID("_Is_Hovered");
        private static readonly int IsSelected = Shader.PropertyToID("_Is_Selected");
        private static readonly int MainTex = Shader.PropertyToID("_MainTex");
        private void Awake()
        {
            _renderer = GetComponent<Renderer>();
            Assert.IsNotNull(_renderer, "_renderer != null");

            
        }

        private void OnDestroy()
        {
            if (_cell == null) return;
            _cell.PropertyChanged -= CellOnPropertyChanged;
        }

        public void SetDataModel(IGridCell cell, ITerrainVariant terrainVariant)
        {
            _cell = cell;
            Bind(cell);
            _materialOverrides = new MaterialPropertyBlock();
            _materialOverrides.SetTexture(MainTex, terrainVariant.TextureOverride);
            _renderer.SetPropertyBlock(_materialOverrides);
        }

        private void Bind(IGridCell cell)
        {
            cell.PropertyChanged += CellOnPropertyChanged;
            name = $"row: {cell.RowIndex}, col: {cell.ColIndex}";
            transform.position = cell.WorldPosition;
        }

        private void CellOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var cell = (IGridCell)sender;

            switch (e.PropertyName)
            {
                case nameof(IGridCell.IsPinned):
                case nameof(IGridCell.IsHighlighted):
                {
                    _materialOverrides.SetFloat(IsHovered, cell.IsHighlighted || cell.IsPinned ? 1 : 0);
                    _renderer.SetPropertyBlock(_materialOverrides);
                    return;
                }
                case nameof(IGridCell.IsSelected):
                {
                    _materialOverrides.SetFloat(IsSelected, cell.IsSelected ? 1 : 0);
                    _renderer.SetPropertyBlock(_materialOverrides);
                    return;
                }
            }
            
        }
    }
}