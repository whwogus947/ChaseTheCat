using UnityEngine;

namespace Com2usGameDev
{
    public class TwinklePoolItem : PoolItem
    {
        public float lifeTime = 1f;

        private float timer;

        public void StartTwinkle()
        {
            timer = lifeTime;
        }

        void Update()
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                onReturnToPool?.Invoke(this);
            }
        }
    }
}
