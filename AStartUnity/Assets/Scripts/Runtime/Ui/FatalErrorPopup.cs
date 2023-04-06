using System;
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

        private void Awake()
        {
            EventSubscriber.OnGameFatalError()
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

        public void QuitApp()
        {
            Application.Quit();
        }
    }
}