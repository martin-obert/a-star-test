using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Runtime.Grid.Services;
using UnityEngine;

namespace Runtime.Gameplay
{
    public sealed class GameManager : MonoBehaviour
    {
        private void Start()
        {
            PreloadAddressable();
        }

        private void PreloadAddressable()
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
                    Console.WriteLine(e);
                    throw;
                    
                }
            });
        }
    }
}