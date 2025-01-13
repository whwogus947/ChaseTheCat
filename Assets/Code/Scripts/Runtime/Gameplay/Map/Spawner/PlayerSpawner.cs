using UnityEngine;

namespace Com2usGameDev
{
    public class PlayerSpawner : MapSpawner
    {
        public TransformChannelSO playerTransformChannel;

        private void Start()
        {
            playerTransformChannel.Invoke(transform);
        }
    }
}
