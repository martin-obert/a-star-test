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

        private readonly CompositeDisposable _disposable = new();
        private int _parsedRows;
        private int _parsedCols;

        private void Awake()
        {
            Assert.IsNotNull(startButton, "startButton != null");
            Assert.IsNotNull(rowsInput, "rowsInput != null");
            Assert.IsNotNull(colsInput, "colsInput != null");

            ParseInputs();

            gameObject.SetActive(false);

            rowsInput.onValueChanged.AsObservable()
                .Merge(
                    colsInput.onValueChanged.AsObservable())
                .Subscribe(_ => { ParseInputs(); }).AddTo(_disposable);

            EventSubscriber.OnPreloadComplete()
                .ObserveOnMainThread()
                .Subscribe(_ => gameObject.SetActive(true))
                .AddTo(_disposable);

            EventSubscriber.OnGridInstantiated()
                .ObserveOnMainThread()
                .Subscribe(_ => gameObject.SetActive(false))
                .AddTo(_disposable);

            startButton.onClick.AsObservable().Subscribe(_ => LoadInGameScene()).AddTo(this);
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
                    GameManager.Instance.GridSetup = new GridSetup
                    {
                        ColCount = _parsedCols,
                        RowCount = _parsedRows
                    };
                    await UniTask.SwitchToMainThread(cSource.Token);
                    await GameManager.Instance.LoadHexWorldAsync(cSource.Token);
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                    EventPublisher.OnGameFatalError("Failed to load scene");
                }
            });
        }

        private void OnDestroy()
        {
            _disposable?.Dispose();
        }
    }
}