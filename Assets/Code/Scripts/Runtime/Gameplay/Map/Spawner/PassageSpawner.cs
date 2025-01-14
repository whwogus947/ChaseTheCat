using UnityEngine;

namespace Com2usGameDev
{
    public class PassageSpawner : MapSpawner
    {
        public override GameObject Spawn(GameObject spawnable)
        {
            var clone = Instantiate(spawnable);
            clone.transform.position = transform.position;
            return clone;
        }
    }
}
