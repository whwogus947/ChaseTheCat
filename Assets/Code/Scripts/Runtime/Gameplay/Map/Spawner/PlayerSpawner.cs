using UnityEngine;

namespace Com2usGameDev
{
    public class PlayerSpawner : MapSpawner
    {
        public TransformChannelSO playerTransformChannel;

        public override void Spawn(GameObject spawnable)
        {
            playerTransformChannel.Invoke(transform);
        }
    }
}
