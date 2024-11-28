using UnityEngine;

namespace Com2usGameDev.Dev
{
    public class UnitBehaviour : MonoBehaviour
    {
        public LinearStatSO direction;
        public Transform unitImage;
        public float walk;
        public float run;
        public float jump;

        private Animator ani;
        private Rigidbody2D rb;
        private int UnitDirection => GetDirection();
        private float transitionPower;

        private void Awake()
        {
            ani = GetComponentInChildren<Animator>();
            rb = GetComponent<Rigidbody2D>();
        }

        void Start()
        {

        }

        void Update()
        {

        }

        public void SetTransitionPower(float power) => transitionPower = power;

        public void VelocityX()
        {
            rb.linearVelocityX = UnitDirection * transitionPower;
        }

        public void Jump(float power)
        {
            rb.AddForceY(power);
        }

        public void PlayAnimation(int animHash, float transitionRate = 0f)
        {
            if (transitionRate > 0)
            {
                ani.CrossFade(animHash, transitionRate);
                return;
            }
            ani.Play(animHash);
        }

        public void SetAnimation(string animHash, bool value)
        {
            ani.SetBool(animHash, value);
        }

        private int GetDirection()
        {
            int directionValue = (int)direction.value;
            if (directionValue == TransformToInt() * -1)
            {
                unitImage.localScale = new(unitImage.localScale.x * -1, 1, 1);
            }
            return directionValue;
        }

        private int TransformToInt()
        {
            return unitImage.localScale.x > 0 ? -1 : 1;
        }
    }
}
