using UnityEngine;

namespace Com2usGameDev
{
    public abstract class MapSpawner : MonoBehaviour
    {
        public abstract void Spawn(GameObject spawnable);
    }

    public interface ISpawnable
    {
        GameObject Spawnable { get; }
    }
}
