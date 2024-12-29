using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace Com2usGameDev
{
    [CreateAssetMenu(fileName = "VFX Pool", menuName = "Cum2usGameDev/Ability/VFXPool")]
    public class VFXPool : ScriptableObject
    {
        private Dictionary<PoolItem, ObjectPool<PoolItem>> pools = new();
        private GameObject frontman;

        [SerializeField] private int maxPoolSize = 15;
        private PoolItem current;

        public PoolItem GetPooledObject(PoolItem target)
        {
            current = target;
            if (frontman == null || !pools.ContainsKey(current))
            {
                AddNewPool(current);
            }
            return pools[current].Get();
        }

        private void ReleasePooledObject(PoolItem obj)
        {
            pools[current].Release(obj);
        }

        private void Initiate()
        {
            frontman = new GameObject("Pool Frontman");
            pools = new();
        }

        private void AddNewPool(PoolItem key)
        {
            if (frontman == null)
            {
                Initiate();
            }
            if (pools.ContainsKey(key))
            {
                return;
            }

            var pool = new ObjectPool<PoolItem>(
                createFunc: CreatePooledObject,
                actionOnGet: OnGetFromPool,
                actionOnRelease: OnReleaseToPool,
                actionOnDestroy: OnDestroyPoolObject,
                collectionCheck: true,
                defaultCapacity: 10,
                maxSize: maxPoolSize
            );
            pools.Add(key, pool);            
        }

        private PoolItem CreatePooledObject()
        {
            PoolItem obj = Instantiate(current);
            obj.onReturnToPool += ReleasePooledObject;
            return obj;
        }

        private void OnGetFromPool(PoolItem obj)
        {
            obj.gameObject.SetActive(true);
        }

        private void OnReleaseToPool(PoolItem obj)
        {
            obj.gameObject.SetActive(false);
        }

        private void OnDestroyPoolObject(PoolItem obj)
        {
            Destroy(obj);
        }
    }
}
