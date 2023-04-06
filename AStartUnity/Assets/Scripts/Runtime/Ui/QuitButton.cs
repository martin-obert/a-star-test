using UnityEngine;

namespace Runtime.Ui
{
    public sealed class QuitButton : MonoBehaviour
    {
        public void TriggerQuit()
        {
            Application.Quit();
        }
    }
}