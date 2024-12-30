using System.Collections.Generic;
using UnityEngine;

namespace Com2usGameDev
{
    public class SlingshotController : MonoBehaviour, IOffensiveWeapon
    {
        public LineRenderer[] lines;
        public Transform handlePrefab;
        public float LineDrawingTimer { get; set; } = 0f;
        public SlingshotBullet slingshotBullet;
        public int poolSize = 10;
        public float bulletSpeed = 10;
        public float bulletLifetime = 1f;
        public VFXPool pool;
        public PoolItem hitFX;

        public AbilityController controller;
        public SlingshotCountSkillSO ammoCountPassive;
        public SlingshotDamageSkillSO damagePassive;
        public SlingshotRangeSkillSO rangePassive;

        private Queue<SlingshotBullet> objectPool = new();
        private Transform handleStorage;

        void Start()
        {
            handleStorage = GetComponentInParent<WeaponPlacer>().leftHand;
            DrawLines();
            ResetLines();

            controller.AddAbility(ammoCountPassive);
            controller.AddAbility(damagePassive);
            controller.AddAbility(rangePassive);

            for (int i = 0; i < poolSize; i++)
            {
                SlingshotBullet obj = Instantiate(slingshotBullet);
                obj.gameObject.SetActive(false);
                objectPool.Enqueue(obj);
            }
        }

        void Update()
        {
            if (LineDrawingTimer > 0)
            {
                LineDrawingTimer -= Time.deltaTime;
                DrawLines();
                if (LineDrawingTimer <= 0)
                {
                    ResetLines();
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

        public void Attack(Vector2 from, Vector2 to, LayerMask layer, int defaultDamage)
        {
            int count = ammoCountPassive.Count;
            var directions = GetSpreadVectors(count, count * 5);

            foreach (var direction in directions)
            {
                var bullet = GetFromPool(handlePrefab.position);
                bullet.Initialize(bulletSpeed, direction * to.x, layer, bulletLifetime, ReturnToPool);
            }
        }

        private Vector2[] GetSpreadVectors(int count, float spreadAngle)
        {
            Vector2[] directions = new Vector2[count];

            if (count <= 1)
            {
                directions[0] = Vector2.right;
                return directions;
            }

            float totalAngle = spreadAngle;
            float startAngle = -totalAngle / 2f;
            float angleStep = totalAngle / (count - 1);

            for (int i = 0; i < count; i++)
            {
                float angle = startAngle + (angleStep * i);
                angle = Random.Range(0, angle);
                float radian = angle * Mathf.Deg2Rad;

                directions[i] = new Vector2(
                    Mathf.Cos(radian),
                    Mathf.Sin(radian)
                );
            }

            return directions;
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
                SlingshotBullet obj = Instantiate(slingshotBullet, position, Quaternion.identity);
                return obj;
            }
        }

        private void ReturnToPool(SlingshotBullet obj)
        {
            obj.AddHitEvent(() =>
            {
                var fx = pool.GetPooledObject(hitFX);
                fx.transform.position = obj.transform.position;
            });
            obj.gameObject.SetActive(false);
            objectPool.Enqueue(obj);
        }
    }
}
