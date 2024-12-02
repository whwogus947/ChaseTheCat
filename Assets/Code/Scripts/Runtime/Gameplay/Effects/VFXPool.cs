using UnityEngine;
using UnityEngine.Pool;

namespace Com2usGameDev
{
    public class VFXPool : MonoBehaviour
    {
        [SerializeField] private PoolItem prefabToPool;

        private ObjectPool<PoolItem> objectPool;

        [SerializeField] private int maxPoolSize = 100;

        private void Awake()
        {
            objectPool = new ObjectPool<PoolItem>(
                createFunc: CreatePooledObject,
                actionOnGet: OnGetFromPool,
                actionOnRelease: OnReleaseToPool,
                actionOnDestroy: OnDestroyPoolObject,
                collectionCheck: true,
                defaultCapacity: 10,
                maxSize: maxPoolSize
            );
        }

        private PoolItem CreatePooledObject()
        {
            PoolItem obj = Instantiate(prefabToPool, transform);
            return obj;
        }

        private void OnGetFromPool(PoolItem obj)
        {
            obj.gameObject.SetActive(true);
            obj.transform.position = obj.GetOffset();
        }

        private void OnReleaseToPool(PoolItem obj)
        {
            obj.gameObject.SetActive(false);
        }

        private void OnDestroyPoolObject(PoolItem obj)
        {
            Destroy(obj);
        }

        public PoolItem GetPooledObject()
        {
            return objectPool.Get();
        }

        public void ReleasePooledObject(PoolItem obj)
        {
            objectPool.Release(obj);
        }

        public PoolItem GetPooledObject(Vector3 position, Quaternion rotation)
        {
            PoolItem obj = objectPool.Get();
            obj.transform.position = position;
            obj.transform.rotation = rotation;
            return obj;
        }

        private void OnDestroy()
        {
            objectPool.Dispose();
        }
    }
}
