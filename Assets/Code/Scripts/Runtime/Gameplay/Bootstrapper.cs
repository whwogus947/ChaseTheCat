using UnityEngine;

namespace Com2usGameDev
{
    public class Bootstrapper
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void OnGameStart()
        {
            var initializeableSO = Resources.LoadAll<ResettableSO>("");
            foreach (var so in initializeableSO)
            {
                so.Initialize();
            }

            var managers = Resources.LoadAll<Manager>("");
            foreach (var manager in managers)
            {
                var clone = Object.Instantiate(manager);
                Object.DontDestroyOnLoad(clone);
            }
        }
    }
}
