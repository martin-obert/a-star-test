using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Runtime.Messaging;
using Runtime.Messaging.Events;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace Runtime.Gameplay
{
    public sealed class GameManager : MonoBehaviour, IGameManager
    {
        [SerializeField] private AssetLabelReference preloadLabel;
        [SerializeField] private AssetReference worldToLoad;
        public static IGameManager Instance { get; private set; }
        private void Awake()
        {
            if (Instance != null)
            {
                if (!ReferenceEquals(Instance, this))
                    Destroy(this);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
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
                    await MessageBus.Instance.PublishOnMainThreadAsync(new GameFatalError("Failed to initialize game"));
                }
            });
        }

        private async UniTask ClearCacheAsync(CancellationToken cancellationToken)
        {
            if (!Application.isEditor) return;

            await MessageBus.Instance.PublishOnMainThreadAsync(new GamePreloadingInfo("Clearing addressable cache"));

            await Addressables.ClearDependencyCacheAsync(preloadLabel, true)
                .WithCancellation(cancellationToken);

            await MessageBus.Instance.PublishOnMainThreadAsync(new GamePreloadingInfo("Cache cleared"));
        }

        private async UniTask PreloadDependenciesAsync(CancellationToken cancellationToken)
        {
            await MessageBus.Instance.PublishOnMainThreadAsync(new GamePreloadingInfo("Checking preload dependencies"));

            var downloadSize = await Addressables.GetDownloadSizeAsync(preloadLabel)
                .WithCancellation(cancellationToken);

            if (downloadSize > 0)
            {
                await MessageBus.Instance.PublishOnMainThreadAsync(
                    new GamePreloadingInfo($"Need to download: {downloadSize} bytes"));

                await Addressables.DownloadDependenciesAsync(preloadLabel)
                    .WithCancellation(cancellationToken);
            }
            else
            {
                await MessageBus.Instance.PublishOnMainThreadAsync(new GamePreloadingInfo("Nothing needed to download"));
            }
            
            await MessageBus.Instance.PublishOnMainThreadAsync(new OnPreloadComplete());
        }

        public async UniTask LoadHexWorldAsync(CancellationToken cancellationToken)
        {
            await MessageBus.Instance.PublishOnMainThreadAsync(new GamePreloadingInfo("Loading hex grid"));

            await Addressables.LoadSceneAsync(worldToLoad, LoadSceneMode.Additive)
                .WithCancellation(cancellationToken);

        }
    }
}