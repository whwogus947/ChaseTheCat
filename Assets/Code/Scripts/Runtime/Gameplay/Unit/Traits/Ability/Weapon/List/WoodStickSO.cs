using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Com2usGameDev
{
    [CreateAssetMenu(fileName = "WoodStick", menuName = "Cum2usGameDev/Ability/Weapon/List/WoodStick")]
    public class WoodStickSO : WeaponAbility
    {
        public override string AbilityName => nameof(WoodStickSO);
        public override int AnimationHash => Animator.StringToHash("main-attack");

        public override void OnAquire()
        {
            
        }

        public override void UseWeapon()
        {
            
        }
    }
}
