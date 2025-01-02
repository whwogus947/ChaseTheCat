using UnityEngine;

namespace Com2usGameDev
{
    public class CatnipBall : ThrowableWeapon
    {
        protected override void OnCollisionGround()
        {
            var groundCol = Physics2D.OverlapCircle(transform.position, 0.25f, layers.ground.value);
            if (groundCol != null)
            {
                circleCollider.isTrigger = false;
                Destroy(gameObject, 5f);
            }
        }
    }
}
