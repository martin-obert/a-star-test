using System;
using Runtime.Messaging;
using Runtime.Messaging.Events;
using TMPro;
using UnityEngine;

namespace Runtime.Ui
{
    public sealed class FatalErrorPopup : MonoBehaviour
    {
        private IDisposable _subHook;
        
        [SerializeField] private TMP_Text messageLabel;

        private void Awake()
        {
            _subHook = MessageBus.Instance.Subscribe<GameFatalError>(x =>
            {
                gameObject.SetActive(true);
                messageLabel.text = x.Message;
            });
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