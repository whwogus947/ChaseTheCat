using UnityEngine;

namespace Com2usGameDev
{
    public class NPCSpawner : MapSpawner
    {
        public override void Spawn(GameObject spawnable)
        {
            var clone = Instantiate(spawnable);
            clone.transform.position = transform.position;
        }
    }
}
