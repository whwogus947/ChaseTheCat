using System.Collections.Generic;
using UnityEngine;

namespace Com2usGameDev
{
    public class BowController : OffensiveWeapon
    {
        public LineRenderer[] lines;
        public Transform handlePrefab;
        public float lineDrawingTimer = 0f;
        public SlingshotBullet arrow;
        public float bulletSpeed = 10;
        public float bulletLifetime = 1f;
        public VFXPool pool;
        public PoolItem hitFX;

        private Queue<SlingshotBullet> objectPool = new();
        public Transform handleStorage;

        private Vector2 direction;
        private LayerMask layer;

        void Start()
        {
            DrawLines();
            ResetLines();

            // for (int i = 0; i < poolSize; i++)
            // {
            //     SlingshotBullet obj = Instantiate(slingshotBullet);
            //     obj.gameObject.SetActive(false);
            //     objectPool.Enqueue(obj);
            // }
        }

        void Update()
        {
            if (lineDrawingTimer > 0)
            {
                lineDrawingTimer -= Time.deltaTime;
                DrawLines();
                if (lineDrawingTimer <= 0)
                {
                    ResetLines();
                    var bullet = GetFromPool(handlePrefab.position);
                    bullet.Initialize(bulletSpeed, direction, layer, bulletLifetime, ReturnToPool);
                }
            }
        }

        private void DrawLines()
        {
            if (handlePrefab.parent != handleStorage)
            {
                handlePrefab.SetParent(handleStorage);
                handlePrefab.localPosition = Vector3.zero;
            }
            for (int i = 0; i < lines.Length; i++)
            {
                lines[i].SetPosition(0, handleStorage.position);
                lines[i].SetPosition(1, lines[i].transform.position);
            }
        }

        private void ResetLines()
        {
            for (int i = 0; i < lines.Length; i++)
            {
                lines[i].SetPosition(0, Vector3.zero);
            }
            for (int i = 0; i < lines.Length; i++)
            {
                Vector3 localPosition = lines[(i + 1) % 2].GetPosition(0) - lines[i].GetPosition(0);
                lines[i].SetPosition(0, Vector3.zero);
                lines[i].SetPosition(1, localPosition);
            }
            handlePrefab.SetParent(transform);
            handlePrefab.position = (lines[0].transform.position + lines[1].transform.position) / 2f;
        }

        public override void Attack(Vector2 from, Vector2 to, LayerMask layer, int defaultDamage)
        {
            lineDrawingTimer = 0.4f;
            this.layer = layer;
            direction = to;
        }

        private SlingshotBullet GetFromPool(Vector3 position)
        {
            if (objectPool.Count > 0 && objectPool.Peek() != null)
            {
                SlingshotBullet obj = objectPool.Dequeue();
                obj.gameObject.SetActive(true);
                obj.transform.position = position;
                obj.transform.rotation = Quaternion.identity;
                return obj;
            }
            else
            {
                for (int i = 0; i < objectPool.Count; i++)
                {
                    var bullet = objectPool.Peek();
                    if (bullet == null)
                        objectPool.Dequeue();
                }
                SlingshotBullet obj = Instantiate(arrow, position, Quaternion.identity);
                return obj;
            }
        }

        private void ReturnToPool(SlingshotBullet obj)
        {
            obj.AddHitEvent(() =>
            {
                if (hitFX != null)
                {
                    var fx = pool.GetPooledObject(hitFX);
                    fx.transform.position = obj.transform.position;
                }
            });
            obj.gameObject.SetActive(false);
            objectPool.Enqueue(obj);
        }
    }
}
