using UnityEngine;

namespace Runtime.Ui
{
    public sealed class QuitButton : MonoBehaviour
    {
        public void TriggerQuit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            return;
#endif
            Application.Quit();
        }
    }
}