using UnityEngine;

namespace Com2usGameDev
{
    public class PassageSpawner : MapSpawner
    {
        public override void Spawn(GameObject spawnable)
        {
            var clone = Instantiate(spawnable);
            clone.transform.position = transform.position;
        }
    }
}
