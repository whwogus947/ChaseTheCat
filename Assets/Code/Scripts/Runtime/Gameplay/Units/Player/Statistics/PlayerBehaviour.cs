using UnityEngine;

namespace Com2usGameDev
{
    public class PlayerBehaviour : UnitBehaviour
    {
        public override void Jump(float power)
        {
            rb.AddForceY(power, ForceMode2D.Force);
        }

        public override void MoveX(float power)
        {
            int isFlip = power > 0 ? -1 : power < 0 ? 1 : (int)child.localScale.x;
            child.localScale = new(isFlip, 1, 1);
            rb.linearVelocityX = power;
        }
    }
}
