using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Runtime.Grid.Services;
using UnityEngine;

namespace Runtime.Ui
{
    public sealed class SaveGridLayoutButton : MonoBehaviour
    {
        private readonly CancellationTokenSource _cSource = new(TimeSpan.FromSeconds(30));

        private void OnDestroy()
        {
            _cSource.Dispose();
        }

        public void SaveLayout()
        {
            UniTask.Void(async () =>
            {
                var cells = ServiceInjector.Instance.GridService.Cells;
                await ServiceInjector.Instance.GridLayoutRepository.SaveAsync(cells, _cSource.Token);
            });
        }
    }
}