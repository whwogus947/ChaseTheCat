using UnityEngine;
using UnityEngine.Events;

namespace Com2usGameDev
{
    public class PoolItem : MonoBehaviour
    {
        public bool isFixed;
        public UnityAction<PoolItem> onReturnToPool;
    }
}
