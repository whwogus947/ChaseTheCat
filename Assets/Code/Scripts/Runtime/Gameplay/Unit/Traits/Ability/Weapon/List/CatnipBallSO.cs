using UnityEngine;

namespace Com2usGameDev
{
    [CreateAssetMenu(fileName = "Catnip Ball", menuName = "Cum2usGameDev/Ability/Weapon/List/CatnipBall")]
    public class CatnipBallSO : WeaponAbilitySO
    {
        public override int AnimationHash => Animator.StringToHash("main-attack");
        public override string AbilityName => nameof(CatnipBallSO);
        public int initialCount;

        private CatnipBall catnipBall;

        public override void OnAquire()
        {
            Count = initialCount;
        }

        public override void UseWeapon()
        {
            if (catnipBall == null)
                catnipBall = weaponOnHand.GetComponent<CatnipBall>();
            
            TakeOne();
        }
    }
}
