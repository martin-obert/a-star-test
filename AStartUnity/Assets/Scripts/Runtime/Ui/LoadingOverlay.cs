using Runtime.Messaging;
using Runtime.Messaging.Events;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

namespace Runtime.Ui
{
    public sealed class LoadingOverlay : MonoBehaviour
    {
        [SerializeField] private TMP_Text infoLabel;
        private readonly CompositeDisposable _disposable = new();

        private void Awake()
        {
            Assert.IsNotNull(infoLabel, "infoLabel != null");
            
            MessageBus.Instance
                .Subscribe<GamePreloadingInfo>(x => infoLabel.text = x.Message)
                .AddTo(_disposable);
            MessageBus.Instance
                .Subscribe<OnPreloadComplete>(x => gameObject.SetActive(false))
                .AddTo(_disposable);
        }

        private void OnDisable()
        {
            _disposable?.Dispose();
        }
    }
}