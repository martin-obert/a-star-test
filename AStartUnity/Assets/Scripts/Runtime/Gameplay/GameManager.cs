using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Runtime.Grid.Services;
using Runtime.Messaging;
using Runtime.Messaging.Events;
using UniRx;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace Runtime.Gameplay
{
    public sealed class GameManager : MonoBehaviour, IGameManager
    {
        [SerializeField] private AssetLabelReference preloadLabel;
        [SerializeField] private AssetReference worldToLoad;

        private EventPublisher _eventPublisher;

        private void Awake()
        {
            UnitOfWork.Instance.RegisterService<IGameManager>(this);
        }

        private void Start()
        {
            _eventPublisher = UnitOfWork.Instance.EventPublisher;
            PreDownloadAssets();
        }

        private void PreDownloadAssets()
        {
            UniTask.Void(async () =>
            {
                using var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromMinutes(3));
                try
                {
                    await UniTask.SwitchToMainThread();
                    await ClearCacheAsync(cancellationTokenSource.Token);
                    await PreloadDependenciesAsync(cancellationTokenSource.Token);
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                    _eventPublisher.OnGameFatalError("Failed to initialize game");
                }
            });
        }

        private async UniTask ClearCacheAsync(CancellationToken cancellationToken)
        {
            if (!Application.isEditor) return;

            _eventPublisher.OnGamePreloadingInfo("Clearing addressable cache");
            
            await Addressables.ClearDependencyCacheAsync(preloadLabel, true)
                .WithCancellation(cancellationToken);

            _eventPublisher.OnGamePreloadingInfo("Cache cleared");
        }

        private async UniTask PreloadDependenciesAsync(CancellationToken cancellationToken)
        {
            _eventPublisher.OnGamePreloadingInfo("Checking preload dependencies");

            var downloadSize = await Addressables.GetDownloadSizeAsync(preloadLabel)
                .WithCancellation(cancellationToken);

            if (downloadSize > 0)
            {
                _eventPublisher.OnGamePreloadingInfo($"Need to download: {downloadSize} bytes");

                await Addressables.DownloadDependenciesAsync(preloadLabel)
                    .WithCancellation(cancellationToken);
            }
            else
            {
                _eventPublisher.OnGamePreloadingInfo("Nothing needed to download");
            }
            
            _eventPublisher.OnPreloadComplete();
        }

        public async UniTask LoadHexWorldAsync(CancellationToken cancellationToken)
        {
            MessageBroker.Default.Publish(new GamePreloadingInfo("Loading hex grid"));

            await Addressables.LoadSceneAsync(worldToLoad, LoadSceneMode.Additive)
                .WithCancellation(cancellationToken);

        }

    }
}