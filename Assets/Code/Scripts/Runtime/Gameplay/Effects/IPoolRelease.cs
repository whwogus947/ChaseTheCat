using UnityEngine;

namespace Com2usGameDev
{
    public interface IPoolRelease
    {
        void ReleasePooledObject(GameObject obj);
    }
}
