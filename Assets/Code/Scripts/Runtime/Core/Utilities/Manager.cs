using UnityEngine;

namespace Com2usGameDev
{
    public class Manager : MonoBehaviour, IInitializeComponent
    {
        public void Initialize()
        {
            var clone = Instantiate(gameObject);
            DontDestroyOnLoad(clone);
        }
    }
}
