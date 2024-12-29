using UnityEngine;

namespace Com2usGameDev
{
    public class BackgroundIllust : MonoBehaviour
    {
        private void Start()
        {
            FindAnyObjectByType<PlayerVisionController>().Background = transform;
        }
    }
}
