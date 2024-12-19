#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Com2usGameDev
{
    public class SystemManager : MonoBehaviour
    {
        public void ExitProgram()
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
