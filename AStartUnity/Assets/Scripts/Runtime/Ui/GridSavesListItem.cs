using System;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace Runtime.Ui
{
    [RequireComponent(typeof(Button))]
    public sealed class GridSavesListItem : MonoBehaviour
    {
        [SerializeField] private TMP_Text label;
        [SerializeField] private Button button;

        private void Awake()
        {
            Assert.IsNotNull(label, "label != null");
            Assert.IsNotNull(button, "button != null");
        }

        public void Bind(string save, Action<string> onClick)
        {
            label.text = Path.GetFileNameWithoutExtension(save);
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => onClick(save));
        }
    }
}