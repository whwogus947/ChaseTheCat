using UnityEngine;

namespace Com2usGameDev
{
    public struct WeaponPerformInfo
    {
        public Vector2 from;
        public Vector2 to;
        public LayersSO layers;
        public int damage;

        public WeaponPerformInfo(Vector2 from, Vector2 to, LayersSO layers, int damage)
        {
            this.from = from;
            this.to = to;
            this.layers = layers;
            this.damage = damage;
        }
    }
}
