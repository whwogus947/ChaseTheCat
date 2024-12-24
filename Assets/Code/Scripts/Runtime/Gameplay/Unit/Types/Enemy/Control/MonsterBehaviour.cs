using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Com2usGameDev
{
    public class MonsterBehaviour : UnitBehaviour
    {
        public LayerMask playerLayer;
        public float attackRange;
        [Header("Sample")]
        

        private Transform player;

        public override bool Controllable { get => enabled; set => enabled = value; }
        private MaterialPropertyBlock propertyBlock;
        private Renderer[] renderers;
        private bool isDissolveOn = false;

        public override void UseVFX()
        {
            
        }

        protected override void Initialize()
        {
            propertyBlock = new MaterialPropertyBlock();
            renderers = GetComponentsInChildren<Renderer>();
            vanishUI = GetComponentInChildren<VanishSlider>();
        }

        protected override void CheckPerFrame()
        {

        }

        public void UpdateAllRendererProperties(float value)
        {
            foreach (Renderer renderer in renderers)
            {
                renderer.GetPropertyBlock(propertyBlock);

                propertyBlock.SetFloat("_Dissolve", value);

                renderer.SetPropertyBlock(propertyBlock);
            }
        }

        protected override void Dead()
        {
            if (isDissolveOn)
                return;

            GetComponent<Collider2D>().enabled = false;

            isDissolveOn = true;
            DissolveRoutine().Forget();
            Controllable = false;
            vanishUI.OnFadeaway();
        }

        private async UniTaskVoid DissolveRoutine()
        {
            float alpha = 0;
            while (alpha <= 1)
            {
                alpha += Time.deltaTime * 1f;
                UpdateAllRendererProperties(alpha);
                await UniTask.Yield();
            }
            UpdateAllRendererProperties(1);
            Destroy(gameObject);
        }

        protected override int GetVelocityDirection()
        {
            return FacingDirection = player != null ? GetTargetDirection(player.position) : FacingDirection;
        }

        public bool IsChasable()
        {
            return IsGround(Vector3.right * GetVelocityDirection());
        }

        public bool IsMovable(int direction)
        {
            return IsGround(Vector3.right * direction);
        }

        private int GetTargetDirection(Vector3 target)
        {
            return target.x > transform.position.x ? 1 : -1;
        }

        private bool IsGround(Vector3 relativePos)
        {
            var cols = Physics2D.OverlapBox(transform.position + Vector3.down + relativePos, Vector2.one, 0f, groundLayer.value);
            return cols != null;
        }

        public bool IsPlayerNearby()
        {
            var cols = Physics2D.OverlapBox(transform.position, new Vector2(8, 2), 0f, playerLayer.value);
            if (player == null && cols != null)
                player = cols.transform;

            return cols != null;
        }

        public bool IsPlayerBeside()
        {
            var cols = Physics2D.OverlapBox(transform.position, new Vector2(4, 2), 0f, playerLayer.value);
            if (player == null && cols != null)
                player = cols.transform;

            return cols != null&& DistanceX(player.position.x) <= attackRange;
        }

        private float DistanceX(float playerX)
        {
            return Mathf.Abs(playerX - transform.position.x);
        }

        public override void Attack()
        {
            var rayHit = Physics2D.BoxCast((Vector2)transform.position, Vector2.one, 0, FacingDirection * 1f * Vector2.right, 2, playerLayer.value);
            if (rayHit.collider != null && rayHit.collider.TryGetComponent(out PlayerBehaviour behaviour))
            {
                behaviour.HP -= 10;
            }
        }
    }
}
