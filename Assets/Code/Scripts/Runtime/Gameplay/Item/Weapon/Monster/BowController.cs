using UnityEngine;

namespace Com2usGameDev
{
    public class BowController : ProjectileLauncher
    {
        protected override void CreateProjectile(Vector2 from, Vector2 to, LayersSO layer, int defaultDamage)
        {
            PlaySound();

            var bullet = GetFromPool();
            bullet.Initialize(projectile, handle.position, to, layer);
        }
    }
}
