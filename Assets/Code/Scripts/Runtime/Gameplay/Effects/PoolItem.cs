using UnityEngine;

namespace Com2usGameDev
{
    public class PoolItem : MonoBehaviour
    {
        public VFXPool pool;
        public GameObject item;
        public Transform offset;
        
        public Vector3 GetOffset()
        {
            return offset.position;
        }
    }
}
