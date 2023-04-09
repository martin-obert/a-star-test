using System;
using System.ComponentModel;
using Runtime.Grid.Data;
using UnityEngine;
using UnityEngine.Assertions;

namespace Runtime.Grid.Presenters
{
    public interface IGridCellGameObject
    {
        string Name { get; set; }
    }

    public sealed class GridCellGameObject : IGridCellGameObject
    {
        private readonly GameObject _gameObject;

        public GridCellGameObject(GameObject gameObject)
        {
            _gameObject = gameObject;
        }
        public string Name
        {
            get => _gameObject.name;
            set => _gameObject.name = value;
        }
    }

    public interface IGridCellRenderer
    {
        void SetPropertyBlock(MaterialPropertyBlock materialPropertyBlock);
    }

    public interface IGridCellTransform
    {
        Vector3 Position { get; set; }
    }

    public sealed class GridCellTransform : IGridCellTransform
    {
        private readonly Transform _transform;

        public GridCellTransform(Transform transform)
        {
            _transform = transform;
        }
        public Vector3 Position
        {
            get => _transform.position;
            set => _transform.position = value;
        }
    }

    public sealed class GridCellRenderer : IGridCellRenderer
    {
        private readonly Renderer _renderer;

        public GridCellRenderer(Renderer renderer)
        {
            _renderer = renderer ? renderer : throw new ArgumentNullException(nameof(renderer));
        }
        public void SetPropertyBlock(MaterialPropertyBlock materialPropertyBlock)
        {
            _renderer.SetPropertyBlock(materialPropertyBlock);
        }
    }

    public interface IGridCellPresenterController
    {
        void SetMainTexture(Texture texture);
    }
    
    [RequireComponent(typeof(Renderer))]
    public class GridCellPresenter : MonoBehaviour
    {
        private Renderer _renderer;
        private static readonly int IsHovered = Shader.PropertyToID("_Is_Hovered");
        private static readonly int IsSelected = Shader.PropertyToID("_Is_Selected");
        private static readonly int MainTex = Shader.PropertyToID("_MainTex");

        public IGridCellPresenterController InitializeController(IGridCellViewModel cellViewModel)
        {
            if (_controller != null) throw new Exception();
            
            _controller = new Controller(
                cellViewModel, 
                new GridCellRenderer(_renderer), 
                new GridCellTransform(transform),
                new GridCellGameObject(gameObject));

            return _controller;
        }
        
        public class Controller : IGridCellPresenterController, IDisposable
        {
            private readonly IGridCellRenderer _renderer;
            private readonly IGridCellViewModel _cellViewModel;
            private MaterialPropertyBlock _materialOverrides;
            public Controller(
                IGridCellViewModel cellViewModel, 
                IGridCellRenderer renderer, 
                IGridCellTransform transform,
                IGridCellGameObject gridCellGameObject)
            {
                _cellViewModel = cellViewModel ?? throw new ArgumentNullException(nameof(cellViewModel));
                _renderer = renderer ?? throw new ArgumentNullException(nameof(renderer));
                cellViewModel.PropertyChanged += CellOnPropertyChanged;
                gridCellGameObject.Name = $"row: {cellViewModel.RowIndex}, col: {cellViewModel.ColIndex}";
                transform.Position = cellViewModel.WorldPosition;
            }
            
            public void Dispose()
            {
                if (_cellViewModel == null) return;
                _cellViewModel.PropertyChanged -= CellOnPropertyChanged;
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
                }
            }

            public void SetMainTexture(Texture texture)
            {
                _materialOverrides.SetTexture(MainTex, texture);
                _renderer.SetPropertyBlock(_materialOverrides);
            }
        }
        

        private Controller _controller;

        private void Awake()
        {
            _renderer = GetComponent<Renderer>();
            Assert.IsNotNull(_renderer, "_renderer != null");
        }

        private void OnDestroy()
        {
            _controller?.Dispose();
        }
    }
}