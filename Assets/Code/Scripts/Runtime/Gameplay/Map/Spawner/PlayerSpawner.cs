using UnityEngine;

namespace Com2usGameDev
{
    public class PlayerSpawner : MapSpawner
    {
        public TransformChannelSO playerTransformChannel;

        public override GameObject Spawn(GameObject spawnable)
        {
            playerTransformChannel.Invoke(transform);
            return spawnable;
        }
    }
}
