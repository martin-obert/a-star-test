using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Runtime.DependencyInjection;
using UnityEngine;

namespace Runtime.Services
{
    public sealed class GameManager : MonoBehaviour
    {
        private void Start()
        {
            PreloadAddressable();
        }

        /// <summary>
        /// Preload addressable to be ready for usage locally
        /// </summary>
        private static void PreloadAddressable()
        {
            UniTask.Void(async () =>
            {
                using var cSource = new CancellationTokenSource(TimeSpan.FromSeconds(30));

                try
                {
                    var addressableManager = ServiceInjector.Instance.AddressableManager;

                    var token = cSource.Token;

                    if (Application.isEditor)
                    {
                        await addressableManager.ClearDependencyCacheAsync(token);
                    }

                    await addressableManager.DownloadDependenciesAsync(token);

                    ServiceInjector.Instance.EventPublisher.OnPreloadComplete();
                }
                catch (Exception e)
                {
                    ServiceInjector.Instance.EventPublisher.OnGameFatalError(e.Message);
                    Debug.LogException(e);
                }
            });
        }
    }
}