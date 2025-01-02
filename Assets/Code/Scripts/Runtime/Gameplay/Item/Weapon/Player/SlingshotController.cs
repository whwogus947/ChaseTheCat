using System.Collections.Generic;
using UnityEngine;

namespace Com2usGameDev
{
    public class SlingshotController : ProjectileLauncher
    {
        public int poolSize = 10;
        public AbilityController controller;
        [Header("Skill Data")]
        public SlingshotCountSkillSO ammoCountPassive;
        public SlingshotDamageSkillSO damagePassive;
        public SlingshotRangeSkillSO rangePassive;

        // private Queue<Projectile> objectPool = new();

        protected override void OnStart()
        {
            controller.AddAbility(ammoCountPassive);
            controller.AddAbility(damagePassive);
            controller.AddAbility(rangePassive);

            // for (int i = 0; i < poolSize; i++)
            // {
            //     Projectile obj = Instantiate(projectile.prefab);
            //     obj.gameObject.SetActive(false);
            //     objectPool.Enqueue(obj);
            // }
        }

        protected override void OnAwake()
        {
            base.OnAwake();
        }

        protected override void CreateProjectile(Vector2 from, Vector2 to, LayersSO layer, int defaultDamage)
        {
            int count = ammoCountPassive.Count;
            var directions = GetSpreadVectors(count, count * 5);
            PlaySound();

            foreach (var direction in directions)
            {
                var bullet = GetFromPool();
                bullet.Initialize(projectile, handle.position, direction * to.x, layer);
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

        // {
        //     if (objectPool.Count > 0 && objectPool.Peek() != null)
        //     {
        //         Projectile obj = objectPool.Dequeue();
        //         obj.gameObject.SetActive(true);
        //         obj.transform.position = position;
        //         obj.transform.rotation = Quaternion.identity;
        //         return obj;
        //     }
        //     else
        //     {
        //         for (int i = 0; i < objectPool.Count; i++)
        //         {
        //             var bullet = objectPool.Peek();
        //             if (bullet == null)
        //                 objectPool.Dequeue();
        //         }
        //         Projectile obj = Instantiate(projectile.prefab, position, Quaternion.identity);
        //         return obj;
        //     }
        // }

        // private void ReturnToPool(Projectile obj)
        // {
        //     obj.AddHitEvent(() =>
        //     {
        //         var fx = pool.GetPooledObject(hitFX);
        //         fx.transform.position = obj.transform.position;
        //     });
        //     obj.gameObject.SetActive(false);
        //     objectPool.Enqueue(obj);
        // }
    }
}
