using UnityEngine;

namespace Com2usGameDev
{
    public class MapSpawner : MonoBehaviour
    {
        public Vector2 Location => transform.position;
    }

    public interface ISpawnable
    {
        void Spawn(MapSpawner spawner);
    }
}
