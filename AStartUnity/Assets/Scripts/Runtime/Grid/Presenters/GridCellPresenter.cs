﻿using System.ComponentModel;
using Runtime.Grid.Data;
using UnityEngine;
using UnityEngine.Assertions;

namespace Runtime.Grid.Presenters
{
    [RequireComponent(typeof(Renderer))]
    public sealed class GridCellPresenter : MonoBehaviour
    {
        [SerializeField] private Color hoverColor = Color.yellow;
        [SerializeField] private Color selectionColor = Color.green;

        private IGridCell _cell;

        private MaterialPropertyBlock _selectedState;
        private MaterialPropertyBlock _hoverState;
        private MaterialPropertyBlock _normalState;
        private static readonly int BaseColorProp = Shader.PropertyToID("_Color");
        private Renderer _renderer;
        private static readonly int MainTex = Shader.PropertyToID("_MainTex");

        private void Awake()
        {
            _renderer = GetComponent<Renderer>();

            Assert.IsNotNull(_renderer, "_renderer != null");

            _selectedState = new MaterialPropertyBlock();
            _hoverState = new MaterialPropertyBlock();
            _normalState = new MaterialPropertyBlock();

            _selectedState.SetColor(BaseColorProp, selectionColor);
            _hoverState.SetColor(BaseColorProp, hoverColor);
            _normalState.SetColor(BaseColorProp, _renderer.material.GetColor(BaseColorProp));
        }

        private void OnDestroy()
        {
            if (_cell == null) return;
            _cell.PropertyChanged -= CellOnPropertyChanged;
        }

        public void SetDataModel(IGridCell cell)
        {
            _cell = cell;
            Bind(cell);
        }

        private void Bind(IGridCell cell)
        {
            cell.PropertyChanged += CellOnPropertyChanged;
            name = $"row: {cell.RowIndex}, col: {cell.ColIndex}";
            transform.position = cell.WorldPosition;
            _normalState.SetTexture(MainTex, cell.TerrainVariant.ColorTexture);
            _hoverState.SetTexture(MainTex, cell.TerrainVariant.ColorTexture);
            _selectedState.SetTexture(MainTex, cell.TerrainVariant.ColorTexture);
            _renderer.SetPropertyBlock(_normalState);
        }

        private void CellOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var cell = (IGridCell)sender;

            switch (e.PropertyName)
            {
                case nameof(IGridCell.IsHighlighted):
                {
                    UpdateHoverState(cell);
                    return;
                }
                case nameof(IGridCell.IsSelected):
                {
                    UpdateSelectionState(cell);
                    return;
                }
                case nameof(IGridCell.IsPinned):
                {
                    UpdatePinState(cell);
                    return;
                }
            }
        }

        private void UpdatePinState(IGridCell cell)
        {
            if(cell.IsSelected) return;
            if (cell.IsPinned)
            {
                _renderer.SetPropertyBlock(_hoverState);
            }
            else
            {
                UpdateHoverState(cell);
            }
        }

        private void UpdateSelectionState(IGridCell cell)
        {
            if (cell.IsSelected)
            {
                _renderer.SetPropertyBlock(_selectedState);
            }
            else
            {
                UpdateHoverState(cell);
            }
        }

        private void UpdateHoverState(IGridCell cell)
        {
            if (cell.IsSelected || cell.IsPinned) return;
            _renderer.SetPropertyBlock(cell.IsHighlighted ? _hoverState : _normalState);
        }

    }
}