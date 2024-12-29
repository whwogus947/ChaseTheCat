using UnityEngine;

namespace Com2usGameDev
{
    public class BackgroundIllust : MonoBehaviour
    {
        public float heightMultiplier = 0.33f;

        private void Start()
        {
            var vision = FindAnyObjectByType<PlayerVisionController>();
            vision.Background = transform;
            vision.multiplier = heightMultiplier;
        }
    }
}
