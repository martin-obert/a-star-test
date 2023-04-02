using System;
using Runtime.Grid.Data;
using UnityEngine;
using UnityEngine.Assertions;

namespace Runtime.Grid.Presenters
{
    [RequireComponent(typeof(Renderer))]
    public sealed class GridCellPresenter : MonoBehaviour, IGridCellSelectable, IGridCellHoverable
    {
        [SerializeField] private Color hoverColor = Color.yellow;
        [SerializeField] private Color selectionColor = Color.green;

        private IGridCell _cell;

        // TODO: this should be false for non-walkable terrain
        public bool CanHover => true;
        private MaterialPropertyBlock _selectedState;
        private MaterialPropertyBlock _hoverState;
        private MaterialPropertyBlock _normalState;
        private static readonly int BaseColorProp = Shader.PropertyToID("_Color");
        private Renderer _renderer;

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

        public void SetDataModel(IGridCell cell)
        {
            _cell = cell;
            name = $"row: {cell.RowIndex}, col: {cell.ColIndex}";
            transform.position = cell.WorldPosition;
        }

        public bool IsBoxCastHit(Vector2 cursor)
        {
            var position = transform.position;
            var v = cursor.y <= position.z + _cell.HeightHalf &&
                    cursor.y >= position.z - _cell.HeightHalf;

            var h = cursor.x <= position.x + _cell.WidthHalf &&
                    cursor.x >= position.x - _cell.WidthHalf;

            return v && h;
        }

        public bool IsCircleCastHit(Vector2 cursor)
        {
            var position = transform.position;

            return IsPointInsideEllipse(cursor.x, cursor.y, position.x, position.z, _cell.WidthHalf,
                _cell.HeightHalf);
        }

        private static bool IsPointInsideEllipse(double x, double y, double cx, double cy, double a, double b)
        {
            double dx = x - cx;
            double dy = y - cy;
            double value = (dx * dx) / (a * a) + (dy * dy) / (b * b);
            return value <= 1;
        }

        public void OnCursorEnter()
        {
            if(IsSelected) return;
            _renderer.SetPropertyBlock(_hoverState);
        }

        public void OnCursorExit()
        {
            if(IsSelected) return;
            _renderer.SetPropertyBlock(_normalState);
        }

        public bool IsSelected { get; private set; }

        public void ToggleSelection(bool? value = null)
        {
            IsSelected = !IsSelected;
            _renderer.SetPropertyBlock(IsSelected ? _selectedState : _normalState);
        }
    }
}