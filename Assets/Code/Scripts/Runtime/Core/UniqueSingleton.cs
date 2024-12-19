using UnityEngine;

namespace Com2usGameDev
{
    public class UniqueSingleton<T> : MonoBehaviour where T : Component
    {
        private static T instance;

        protected virtual void Initialize() { }

        private void Awake()
        {
            if (instance == null)
            {
                instance = this as T;
                DontDestroyOnLoad(gameObject);
                Initialize();
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }
        }
    }
}
