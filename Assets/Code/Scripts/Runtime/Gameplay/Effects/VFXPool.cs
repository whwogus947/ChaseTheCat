using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace Com2usGameDev
{
    [CreateAssetMenu(fileName = "VFX Pool", menuName = "Cum2usGameDev/Ability/VFXPool")]
    public class VFXPool : ScriptableObject
    {
        private Dictionary<PoolItem, Queue<PoolItem>> pools = new();
        private GameObject frontman;

        [SerializeField] private int maxPoolSize = 15;
        private PoolItem current;

        public PoolItem GetPooledObject(PoolItem target)
        {
            current = target;
            if (frontman == null || !pools.ContainsKey(target))
            {
                AddNewPool(target);
            }
            return OnGetFromPool(target);
        }

        // private void ReleasePooledObject(PoolItem obj)
        // {
        //     pools[current].Release(obj);
        // }

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

            var pool = new Queue<PoolItem>();
            pools.Add(key, pool);     
            pool.Enqueue(CreatePooledObject(key));
        }

        private PoolItem CreatePooledObject(PoolItem key)
        {
            PoolItem clone = Instantiate(key);
            clone.onReturnToPool = (item) => OnReleaseToPool(key, item);
            pools[key].Enqueue(clone);
            return clone;
        }

        private PoolItem OnGetFromPool(PoolItem key)
        {
            if (pools[key].Count < 1)
            {
                CreatePooledObject(key);
            }
            var clone = pools[key].Dequeue();
            clone.gameObject.SetActive(true);
            return clone;
        }

        private void OnReleaseToPool(PoolItem key, PoolItem item)
        {
            item.gameObject.SetActive(false);
            pools[key].Enqueue(item);
        }
    }


    // [CreateAssetMenu(fileName = "VFX Pool", menuName = "Cum2usGameDev/Ability/VFXPool")]
    // public class VFXPool : ScriptableObject
    // {
    //     private Dictionary<PoolItem, ObjectPool<PoolItem>> pools = new();
    //     private GameObject frontman;

    //     [SerializeField] private int maxPoolSize = 15;
    //     private PoolItem current;

    //     public PoolItem GetPooledObject(PoolItem target)
    //     {
    //         current = target;
    //         if (frontman == null || !pools.ContainsKey(target))
    //         {
    //             AddNewPool(target);
    //         }
    //         return pools[target].Get();
    //     }

    //     private void ReleasePooledObject(PoolItem obj)
    //     {
    //         pools[current].Release(obj);
    //     }

    //     private void Initiate()
    //     {
    //         frontman = new GameObject("Pool Frontman");
    //         pools = new();
    //     }

    //     private void AddNewPool(PoolItem key)
    //     {
    //         if (frontman == null)
    //         {
    //             Initiate();
    //         }
    //         if (pools.ContainsKey(key))
    //         {
    //             return;
    //         }

    //         var pool = new ObjectPool<PoolItem>(
    //             createFunc: CreatePooledObject,
    //             actionOnGet: OnGetFromPool,
    //             actionOnRelease: OnReleaseToPool,
    //             actionOnDestroy: OnDestroyPoolObject,
    //             collectionCheck: true,
    //             defaultCapacity: 10,
    //             maxSize: maxPoolSize
    //         );
    //         pools.Add(key, pool);            
    //     }

    //     private PoolItem CreatePooledObject()
    //     {
    //         PoolItem obj = Instantiate(current);
    //         obj.onReturnToPool = (item) => pools[current].Release(obj);
    //         Debug.Log(current);
    //         Debug.Log(obj);
    //         return obj;
    //     }

    //     private void OnGetFromPool(PoolItem obj)
    //     {
    //         obj.gameObject.SetActive(true);
    //     }

    //     private void OnReleaseToPool(PoolItem obj)
    //     {
    //         obj.gameObject.SetActive(false);
    //     }

    //     private void OnDestroyPoolObject(PoolItem obj)
    //     {
    //         Destroy(obj);
    //     }
    // }
}
