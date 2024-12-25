using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Com2usGameDev
{
    [CreateAssetMenu(fileName = "WoodStick", menuName = "Cum2usGameDev/Ability/Weapon/List/WoodStick")]
    public class WoodStickSO : WeaponAbility
    {
        public override string AbilityName => nameof(WoodStickSO);

        // private int animationHash = Animator.StringToHash("Player Attack");
        // private Animator animator;

        public override void OnAquire()
        {
            
        }

        public override void UseWeapon()
        {
            Debug.Log("Wood Stick Attack");
            // FXRoutine().Forget();
        }

        // private async UniTaskVoid FXRoutine()
        // {
        //     await UniTask.WaitForSeconds(0.3f);
        //     if (fxClone == null)
        //         return;
                
        //     fxClone.SetActive(true);
        //     if (animator == null)
        //         animator = fxClone.GetComponent<Animator>();

        //     animator.CrossFade(animationHash, 0.5f);
        //     Debug.Log(animator, animator);
        // }
    }
}
