using Runtime.Messaging;
using Runtime.Messaging.Events;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

namespace Runtime.Ui
{
    public sealed class LoadingOverlay : MonoBehaviour
    {
        [SerializeField] private TMP_Text infoLabel;
        private readonly CompositeDisposable _disposable = new();

        private void Awake()
        {
            Assert.IsNotNull(infoLabel, "infoLabel != null");
            
            EventSubscriber.OnGamePreloadingInfo()
                .ObserveOnMainThread()
                .Subscribe(x => infoLabel.text = x.Message)
                .AddTo(_disposable);
            
            EventSubscriber.OnPreloadComplete()
                .ObserveOnMainThread()
                .Subscribe(x => gameObject.SetActive(false))
                .AddTo(_disposable);
        }

        private void OnDisable()
        {
            _disposable?.Dispose();
        }
    }
}