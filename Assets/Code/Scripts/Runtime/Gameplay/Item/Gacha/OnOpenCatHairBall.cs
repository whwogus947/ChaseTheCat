using UnityEngine;

namespace Com2usGameDev
{
    public class OnOpenCatHairBall : StateMachineBehaviour
    {
        private CatHairBall ball;
        
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (ball == null)
                ball = animator.GetComponentInParent<CatHairBall>();
            
            ball.Open();
        }
    }
}
