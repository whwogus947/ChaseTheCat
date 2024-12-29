#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Com2usGameDev
{
    public class SystemManager : MonoBehaviour
    {
        public int targetFramerate;

        private void Awake()
        {
            Application.targetFrameRate = targetFramerate;
        }

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
