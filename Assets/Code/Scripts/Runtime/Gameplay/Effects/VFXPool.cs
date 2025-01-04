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

        public PoolItem GetPooledObject(PoolItem target)
        {
            if (frontman == null || !pools.ContainsKey(target))
                AddNewPool(target);

            return OnGetFromPool(target);
        }

        public void Visualize(PoolItem fx, Transform storage, int direction)
        {
            if (fx == null)
                return;

            var poolObj = GetPooledObject(fx);
            if (poolObj.isFixed)
            {
                poolObj.transform.SetParent(storage, false);
            }
            else
            {
                bool isFlip = direction == 1;
                poolObj.transform.position = storage.position - fx.transform.position * direction;
                if (poolObj.TryGetComponent(out SpriteRenderer sprite))
                {
                    sprite.flipX = isFlip;
                }
            }
        }

        private void Clear()
        {
            if (pools == null || pools.Count <= 0)
                return;

            foreach (var pool in pools.Values)
            {
                for (int i = 0; i < pool.Count; i++)
                {
                    var item = pool.Dequeue();
                    if (item != null)
                        Destroy(item.gameObject);
                }
            }
        }

        private void Initiate()
        {
            Clear();
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
}
