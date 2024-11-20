using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Com2usGameDev
{
    public class TransitionController : MonoBehaviour
    {
        public InputControllerSO inputController;
        public Animator animController;
        public float speed = 3f;
        public float gravityForce = 9.8f;
        public float jumpPower;
        public Image powerGage;
        public float gageIncrease = 10;
        public float runSpeed = 3.5f;
        private float jumpForce;

        private PCInput input;
        private SpriteRenderer sprite;
        [SerializeField] private float groundRayDistance;
        [SerializeField] private LayerMask groundLayer;
        private bool isGround;
        public int targetFrameRate;
        private Rigidbody2D rigid;
        private bool isOnAir;
        private Vector2 velocityOnJump;
        private bool jumpStart;
        private Camera mainCam;
        private float dashTimer = 0.4f;
        private float timer;
        private float speedX;
        private bool isRun;
        private bool isCrouched;

        void Start()
        {
            Application.targetFrameRate = targetFrameRate;
            input = inputController.GetOrCreate();

            sprite = GetComponent<SpriteRenderer>();
            rigid = GetComponent<Rigidbody2D>();

            input.Player.Jump.performed += OnPressJump;
            input.Player.Jump.canceled += OnStartJump;
            input.Player.Attack.performed += OnAttack;
            input.Player.Dash.performed += OnDash;
            input.Player.Run.performed += OnRunStart;
            input.Player.Run.canceled += OnRunEnd;
            isGround = true;
            isOnAir = false;

            mainCam = Camera.main;
        }

        private void OnRunEnd(InputAction.CallbackContext context)
        {
            isRun = false;
            animController.CrossFade("main-idle", 0.2f);
            // animController.SetBool("IsRun", isRun);
        }

        private void OnRunStart(InputAction.CallbackContext context)
        {
            isRun = true;
            animController.CrossFade("main-run", 0.2f);
            // animController.SetBool("IsRun", isRun);
        }

        private void OnDash(InputAction.CallbackContext context)
        {
            if (timer > 0)
                return;

            timer = dashTimer;

            animController.Play("main-dash");
            var velocity = input.Player.Transit.ReadValue<Vector2>() * speed;
            speedX = velocity.x * 5;
        }

        private void OnAttack(InputAction.CallbackContext context)
        {
            animController.CrossFade("main-attack", 0f);
        }

        private void OnDestroy()
        {
            inputController.Dispose();
        }

        void Update()
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
                rigid.linearVelocityX = speedX;

                return;
            }

            isOnAir = !IsOnGround();
            isGround = !isOnAir;
            if (!isOnAir)
            {
                var velocity = input.Player.Transit.ReadValue<Vector2>() * speed;
                velocity = isRun ? velocity + runSpeed * velocity : isCrouched ? velocity - runSpeed * velocity * 0.5f : velocity;
                rigid.linearVelocityX = velocity.x;
                float isLeft = velocity.x < 0 ? 1 : velocity.x > 0 ? -1 : transform.localScale.x;
                transform.localScale = new Vector3(isLeft, transform.localScale.y, transform.localScale.z);
                bool isWalk = Mathf.Abs(velocity.x) > 0  && input.Player.Run.phase != InputActionPhase.Performed;
                // if (isWalk)
                    // animController.Play("main_walk");
                animController.SetBool("IsWalking", isWalk);
            }

            if (jumpStart && !isOnAir)
            {
                jumpForce += Time.deltaTime * gageIncrease;
                jumpForce = Mathf.Clamp(jumpForce, 0, jumpPower);
                powerGage.fillAmount = jumpForce / jumpPower;
            }
            else if (jumpStart && isOnAir)
            {
                jumpStart = false;
                isOnAir = true;
                isGround = false;
                powerGage.fillAmount = 0;
            }

            var originPos = transform.position;
            var xSize = 8f / 9 * 16;
            Vector3 playerPos = new Vector3(Mathf.FloorToInt((originPos.x + xSize / 2) / xSize) * xSize, Mathf.FloorToInt((originPos.y + 4) / 8f) * 8f, 0);
            playerPos.z = -10;
            mainCam.transform.position = playerPos;
        }

        private void OnPressJump(InputAction.CallbackContext context)
        {
            if (isGround)
            {
                isCrouched = true;
                animController.speed = 0.75f;
                animController.CrossFade("main-jumpcharging", 0.2f);
                jumpForce = 0;
                jumpStart = true;
            }
        }

        private void OnStartJump(InputAction.CallbackContext context)
        {
            Jump();
        }

        private void Jump()
        {
            if (isGround && jumpStart)
            {
                isCrouched = false;
                animController.speed = 1;
                animController.SetBool("IsOnGround", false);
                jumpStart = false;
                isOnAir = true;
                isGround = false;
                rigid.AddForceY(jumpForce, ForceMode2D.Force);
                velocityOnJump = rigid.linearVelocity;
                powerGage.fillAmount = 0;
            }
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            jumpForce = 0;
            if (IsOnGround())
            {
                animController.SetBool("IsOnGround", true);
                Debug.Log("On Ground");
                isOnAir = false;
                isGround = true;
            }
            else
            {
                isOnAir = true;
                rigid.linearVelocityX = -velocityOnJump.x;
                Debug.Log("Next To Wall");
                animController.Play("main-hit");
            }
        }

        private bool IsOnGround()
        {
            // var rayHit = Physics2D.Raycast(offset, Vector2.down, 20f, groundLayer);
            var rayHit = Physics2D.BoxCast(transform.position, Vector2.one * 0.96f, 0, Vector2.down, 20f, groundLayer);
            float distance = float.MaxValue;
            if (rayHit.collider != null)
            {
                distance = rayHit.distance;
            }
            Debug.DrawLine(transform.position, (Vector2)transform.position + Vector2.down * distance, Color.yellow);
            return distance < 0.05f;
        }
    }
}