using System;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Runtime.Gameplay;
using Runtime.Messaging;
using Runtime.Messaging.Events;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace Runtime.Ui
{
    public sealed class MainMenu : MonoBehaviour
    {
        [SerializeField] private Button startButton;

        private readonly CompositeDisposable _disposable = new();

        private void Awake()
        {
            Assert.IsNotNull(startButton, "startButton != null");
            gameObject.SetActive(false);

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

        private void LoadInGameScene()
        {
            startButton.interactable = false;

            UniTask.Void(async () =>
            {
                using var cSource = new CancellationTokenSource(TimeSpan.FromSeconds(5));
                try
                {
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