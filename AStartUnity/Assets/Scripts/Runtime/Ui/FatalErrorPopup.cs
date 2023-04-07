using System;
using Runtime.Grid.Services;
using Runtime.Messaging;
using Runtime.Messaging.Events;
using TMPro;
using UniRx;
using UnityEngine;

namespace Runtime.Ui
{
    public sealed class FatalErrorPopup : MonoBehaviour
    {
        private IDisposable _subHook;
        
        [SerializeField] private TMP_Text messageLabel;

        private EventSubscriber _eventSubscriber;
        
        private void Start()
        {
            _eventSubscriber = Grid.Services.ServiceInjector.Instance.EventSubscriber;

            _subHook?.Dispose();
            
            _eventSubscriber.OnGameFatalError()
                .ObserveOnMainThread()
                .Subscribe(x =>
                {
                    gameObject.SetActive(true);
                    messageLabel.text = x.Message;
                }).AddTo(this);

            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            _subHook?.Dispose();
        }
    }
}