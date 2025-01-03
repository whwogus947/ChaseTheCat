using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Com2usGameDev
{
    public abstract class ProjectileLauncher : OffensiveWeapon
    {
        [Header("Control")]
        public ProjectileSettings projectile;
        public Transform handle;

        [Header("FX")]
        public PoolItem hitFX;

        protected Transform handleStorage;
        protected LineRenderer[] lines;

        private CancellationTokenSource _cts;

        private void Awake()
        {
            lines = GetComponentsInChildren<LineRenderer>();
            OnAwake();
        }

        protected virtual void OnAwake() { }

        private void Start()
        {
            var behaviour = GetComponentInParent<UnitBehaviour>();
            handleStorage = transform.parent == behaviour.hands.left ? behaviour.hands.right : behaviour.hands.left;
            DrawLines();
            OnStart();
        }

        protected virtual void OnStart() { }

        private void OnDisable()
        {
            _cts?.Cancel();
        }

        private void Update()
        {
            DrawLines();
        }

        public override void Use(Vector2 from, Vector2 to, LayersSO layer, int defaultDamage)
        {
            Fire(from, to, layer, defaultDamage).Forget();
        }

        protected abstract void CreateProjectile(Vector2 from, Vector2 to, LayersSO layer, int defaultDamage);

        private async UniTask Fire(Vector2 from, Vector2 to, LayersSO layer, int defaultDamage)
        {
            _cts?.Cancel();
            _cts = new CancellationTokenSource();

            if (handle.parent != handleStorage)
            {
                handle.SetParent(handleStorage);
                handle.localPosition = Vector3.zero;
            }

            float timer = delay;
            while (timer > 0)
            {
                await UniTask.Yield(cancellationToken: _cts.Token);
                timer -= Time.deltaTime;
            }

            int directionX = transform.position.x - handleStorage.position.x > 0 ? 1: -1;
            var direction = new Vector2(directionX, 0);
            CreateProjectile(from, direction, layer, defaultDamage);

            ResetLines();
        }

        private void ResetLines()
        {
            handle.SetParent(transform);
            handle.position = (lines[0].transform.position + lines[1].transform.position) / 2f;
        }

        protected void DrawLines()
        {
            for (int i = 0; i < lines.Length; i++)
            {
                lines[i].SetPosition(0, handle.position);
                lines[i].SetPosition(1, lines[i].transform.position);
            }
        }

        protected Projectile GetFromPool()
        {
            var p = fXPool.GetPooledObject(projectile.prefab);
            var bullet = p as Projectile;
            bullet.transform.position = transform.position;
            // if (hitFX != null)
            //     bullet.onReturnToPool += CreateHitVFX;
            return bullet;
        }

        private void CreateHitVFX(PoolItem poolItem)
        {
            var fx = fXPool.GetPooledObject(hitFX);
            fx.transform.position = poolItem.transform.position;
            Debug.Log("from pool");
        }
    }
}
