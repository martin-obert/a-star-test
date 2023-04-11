using System;
using System.ComponentModel;
using Runtime.Grid.Data;
using UnityEngine;
using UnityEngine.Assertions;

namespace Runtime.Grid.Presenters
{
    [RequireComponent(typeof(Transform))]
    public class GridCellTransformWrapper : MonoBehaviour
    {
        public class Controller : IDisposable
        {
            private readonly IGridCellViewModel _viewModel;
            private readonly IGridCellTransform _transform;
            private Vector3 _initialPosition;
            private Vector3 _liftedPosition;
            private bool _isLifted;
            private float _liftAmount;
            private readonly Configuration _configuration;

            public Controller(IGridCellViewModel viewModel, IGridCellTransform transform, Configuration configuration)
            {
                _viewModel = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
                _transform = transform ?? throw new ArgumentNullException(nameof(transform));
                _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            }

            public void Initialize()
            {
                _viewModel.PropertyChanged += ViewModelOnPropertyChanged;
                var position = _transform.Position;
                _initialPosition = position;
                _liftedPosition = position + Vector3.up * _configuration.LiftAmount;
            }

            private void ViewModelOnPropertyChanged(object sender, PropertyChangedEventArgs e)
            {
                var cell = (IGridCellViewModel)sender;
                if (!cell.IsWalkable) return;
                switch (e.PropertyName)
                {
                    case nameof(IGridCellViewModel.IsHighlighted):
                    case nameof(IGridCellViewModel.IsPinned):
                    case nameof(IGridCellViewModel.IsSelected):
                    {
                        _isLifted = cell.IsSelected || cell.IsHighlighted || cell.IsPinned;
                        break;
                    }
                }
            }

            public void Update(float deltaTime)
            {
                _liftAmount = _isLifted
                    ? Mathf.Clamp01(_liftAmount + deltaTime)
                    : Mathf.Clamp01(_liftAmount - deltaTime);
                _transform.Position = Vector3.Lerp(_initialPosition, _liftedPosition, _liftAmount);
            }

            public void Dispose()
            {
                _viewModel.PropertyChanged -= ViewModelOnPropertyChanged;
            }
        }

        [Serializable]
        public class Configuration
        {
            [SerializeField] private float liftAmount;

            public virtual float LiftAmount => liftAmount;
        }

        private Animator _animator;
        private Controller _controller;

        [SerializeField] private Configuration componentConfiguration;
        [SerializeField] private GridCellFacade facade;

        private void Awake()
        {
            Assert.IsNotNull(facade, "facade != null");
        }

        private void Start()
        {
            _controller = new Controller(facade.ViewModel, new GridCellTransform(transform), componentConfiguration);
            _controller.Initialize();
        }

        private void Update()
        {
            _controller?.Update(Time.deltaTime);
        }

        private void OnDestroy()
        {
            _controller?.Dispose();
        }
    }
}