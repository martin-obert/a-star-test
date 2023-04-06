using System;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Runtime.Gameplay;
using Runtime.Messaging;
using Runtime.Messaging.Events;
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
            MessageBus.Instance.Subscribe<OnPreloadComplete>(_ => gameObject.SetActive(true)).AddTo(_disposable);
            MessageBus.Instance.Subscribe<OnGridInstantiated>(_ => gameObject.SetActive(false)).AddTo(_disposable);

            startButton.onClick.AddListener(LoadInGameScene);
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
                    await MessageBus.Instance.PublishOnMainThreadAsync(new GameFatalError("Failed to load scene"));
                }
            });
        }

        private void OnDestroy()
        {
            _disposable?.Dispose();
        }
    }
}