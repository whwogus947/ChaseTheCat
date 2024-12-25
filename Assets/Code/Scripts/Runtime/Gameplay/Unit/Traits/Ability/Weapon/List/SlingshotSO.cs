using UnityEngine;

namespace Com2usGameDev
{
    [CreateAssetMenu(fileName = "Slingshot", menuName = "Cum2usGameDev/Ability/Weapon/List/Slingshot")]
    public class SlingshotSO : WeaponAbility
    {
        public override string AbilityName => nameof(SlingshotSO);

        // private int animationHash = Animator.StringToHash("Player Attack");
        // private Animator animator;

        public override void OnAquire()
        {
            
        }

        public override void UseWeapon()
        {
            // fxClone.SetActive(true);
            // if (animator == null)
            //     animator = fxClone.GetComponent<Animator>();
            
            // animator.CrossFade(animationHash, 0.2f);
            // Debug.Log("Wood Stick Attack");
        }
    }
}
