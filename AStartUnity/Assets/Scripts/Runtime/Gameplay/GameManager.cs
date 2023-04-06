using System;
using System.Collections;
using Runtime.Definitions;
using Runtime.Messaging;
using Runtime.Messaging.Events;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

namespace Runtime.Gameplay
{
    public sealed class GameManager : MonoBehaviour, IGameManager
    {
        [SerializeField] private AssetLabelReference preloadLabel;
        [SerializeField] private AssetReference worldToLoad;

        private Exception _preloadException;

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

        private IEnumerator Start()
        {
            yield return ClearCacheIterator();
            yield return PreloadDependenciesIterator();
            yield return LoadHexWorldIterator();
        }

        private IEnumerator ClearCacheIterator()
        {
            if (!Application.isEditor || !CanContinuePreload()) yield break;

            MessageBus.Instance.Publish(new GamePreloadingInfo("Clearing addressable cache"));

            var y = Addressables.ClearDependencyCacheAsync(preloadLabel, true);
            yield return y;
        }

        private IEnumerator PreloadDependenciesIterator()
        {
            if (!CanContinuePreload()) yield break;

            MessageBus.Instance.Publish(new GamePreloadingInfo("Checking preload dependencies"));

            var downloadSize = Addressables.GetDownloadSizeAsync(preloadLabel);
            yield return downloadSize;

            ThrowIfAsyncOperationFails(downloadSize);
            if (!CanContinuePreload()) yield break;

            if (downloadSize.Result > 0)
            {
                MessageBus.Instance.Publish(new GamePreloadingInfo($"Need to download: {downloadSize.Result}"));

                var dependencyPreloadOperation = Addressables.DownloadDependenciesAsync(preloadLabel);
                yield return dependencyPreloadOperation;

                ThrowIfAsyncOperationFails(dependencyPreloadOperation);
                if (!CanContinuePreload()) yield break;
            }
            else
            {
                MessageBus.Instance.Publish(new GamePreloadingInfo("Nothing needed to download"));
            }
        }

        private IEnumerator LoadHexWorldIterator()
        {
            if (!CanContinuePreload()) yield break;
            MessageBus.Instance.Publish(new GamePreloadingInfo("Loading hex grid"));

            var loadOperation = Addressables.LoadSceneAsync(worldToLoad, LoadSceneMode.Additive);
            yield return loadOperation;

            MessageBus.Instance.Publish(new OnPreloadComplete());
        }

        private void ThrowIfAsyncOperationFails(AsyncOperationHandle operation)
        {
            if (operation.Status == AsyncOperationStatus.Succeeded) return;
            _preloadException =
                new Exception($"Unable to continue, async operation failed. OperationStatus={operation.Status}");
        }

        private bool CanContinuePreload()
        {
            return _preloadException == null;
        }
    }
}