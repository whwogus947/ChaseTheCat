using UnityEngine;

namespace Com2usGameDev
{
    public class EnemySpawner : MapSpawner
    {
        public override void Spawn(GameObject spawnable)
        {
            var clone = Instantiate(spawnable);
            clone.transform.position = transform.position;
        }
    }
}
