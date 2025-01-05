using UnityEngine;

namespace Com2usGameDev
{
    public class Bootstrapper
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void OnGameStart()
        {
            var systemLaunchers = Resources.LoadAll<SystemLauncherSO>("");
            foreach (var launcher in systemLaunchers)
            {
                launcher.Initiate();
            }
        }
    }
}
