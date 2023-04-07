using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Runtime.Gameplay;
using Runtime.Grid.Services;
using Runtime.Messaging;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace Runtime.Ui
{
    public sealed class MainMenu : MonoBehaviour
    {
        [SerializeField] private Button startButton;
        [SerializeField] private TMP_InputField rowsInput;
        [SerializeField] private TMP_InputField colsInput;
        
        private readonly CompositeDisposable _disposable= new();

        private IGameManager _gameManager;
        private EventPublisher _eventPublisher;

        private int _parsedRows;
        private int _parsedCols;

        private void Awake()
        {
            Assert.IsNotNull(startButton, "startButton != null");
            Assert.IsNotNull(rowsInput, "rowsInput != null");
            Assert.IsNotNull(colsInput, "colsInput != null");
        }

        private void Start()
        {
            _gameManager = UnitOfWork.Instance.GameManager;
            _eventPublisher = UnitOfWork.Instance.EventPublisher;
            var eventSubscriber = UnitOfWork.Instance.EventSubscriber;
            
            ParseInputs();

            rowsInput.onValueChanged.AsObservable()
                .Merge(
                    colsInput.onValueChanged.AsObservable())
                .Subscribe(_ => { ParseInputs(); }).AddTo(_disposable);

            eventSubscriber.OnPreloadComplete()
                .ObserveOnMainThread()
                .Subscribe(_ => gameObject.SetActive(true))
                .AddTo(_disposable);

            eventSubscriber.OnGridInstantiated()
                .ObserveOnMainThread()
                .Subscribe(_ => gameObject.SetActive(false))
                .AddTo(_disposable);

            startButton.onClick.AsObservable().Subscribe(_ => LoadInGameScene()).AddTo(this);

            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            _disposable?.Dispose();
        }

        private void ParseInputs()
        {
            _parsedRows = 0;
            _parsedCols = 0;
            int.TryParse(rowsInput.text, out _parsedRows);
            int.TryParse(colsInput.text, out _parsedCols);
            startButton.interactable = _parsedRows > 0 && _parsedCols > 0;
        }

        private void LoadInGameScene()
        {
            startButton.interactable = false;

            UniTask.Void(async () =>
            {
                using var cSource = new CancellationTokenSource(TimeSpan.FromSeconds(5));
                try
                {
                    UnitOfWork.Instance.SceneContextManager.SetContext(new SceneContext
                        {
                            ColCount = _parsedCols,
                            RowCount = _parsedRows
                        }
                    );
                    
                    await UniTask.SwitchToMainThread(cSource.Token);
                    await _gameManager.LoadHexWorldAsync(cSource.Token);
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                    _eventPublisher.OnGameFatalError("Failed to load scene");
                }
            });
        }
    }
}