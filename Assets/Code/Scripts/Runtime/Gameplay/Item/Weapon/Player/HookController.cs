using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Com2usGameDev
{
    public class HookController : OffensiveWeapon
    {
        public float paintSpeed = 1f;
        public Transform hook;
        public int maxRopeLength = 15;

        private LineRenderer line;
        // private bool isHooked;
        // private bool isUnhooked = false;
        // private float ropeLength;
        private Vector3 ropeStartPoint;
        private Transform player;
        // private bool isTransported = true;
        private Rigidbody2D rb;
        private Vector2 hookLocalPosition;
        // private bool isUpperDirection = true;
        // private UnityAction onResetHook;
        private PlayerController controller;
        private LayerMask groundLayer;
        private CancellationTokenSource cts;

        void Awake()
        {
            player = transform.root;
            line = GetComponentInChildren<LineRenderer>();
            rb = player.GetComponent<Rigidbody2D>();
            controller = player.GetComponent<PlayerController>();
            hookLocalPosition = hook.localPosition;
            line.gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            player = transform.root;
            line = GetComponentInChildren<LineRenderer>(true);
            rb = player.GetComponent<Rigidbody2D>();
            controller = player.GetComponent<PlayerController>();
            hookLocalPosition = hook.localPosition;
            line.gameObject.SetActive(false);

            ResetHook();
            cts?.Cancel();
        }

        // void Update()
        // {
        //     if (isHooked)
        //     {
        //         DrawRope();
        //     }
        //     else if (!isTransported)
        //     {
        //         TransportPlayer();
        //     }

        //     if (isUnhooked)
        //     {
        //         DrawUnhookedRope();
        //     }
        // }

        // public void CastRope(UnityAction onReset)
        // public void CastRope()
        // {
        //     var start = InitializeHook();
        //     if (HasHookCollision(start, out Vector2 end))
        //     {
        //         isHooked = true;
        //         ropeLength = Vector2.Distance(start, end);
        //     }
        //     else
        //     {
        //         ropeLength = maxRopeLength;
        //         isUnhooked = true;
        //         isUpperDirection = true;
        //     }
        //     PlaySound();
        // }

        public async UniTask CastRope()
        {
            InitializeHook();
            PlaySound();
            float ropeLength;
            if (HasHookCollision(ropeStartPoint, out Vector2 end))
            {
                ropeLength = Vector2.Distance(ropeStartPoint, end);
                await HookedRoutine(ropeLength, cts.Token);
            }
            else
            {
                ropeLength = maxRopeLength;
                await UnhookedRoutine(ropeLength, cts.Token);
            }
            ResetHook();
        }

        private async UniTask HookedRoutine(float lenth, CancellationToken cancellationToken)
        {
            var ropePainted = 0f;
            while (ropePainted < lenth)
            {
                var speed = Mathf.Min((lenth - ropePainted) * 5f + 0.2f * paintSpeed, paintSpeed);
                ropePainted += speed * Time.deltaTime;
                await UniTask.Yield(cancellationToken);

                if (ropePainted >= lenth)
                {
                    rb.GetComponent<Collider2D>().isTrigger = true;
                    rb.linearVelocity = Vector2.zero;
                }
                hook.position = ropeStartPoint + Vector3.up * ropePainted;
                line.SetPosition(0, hook.position);
                line.SetPosition(1, ropeStartPoint);
            }

            float leftDist = lenth;
            while (leftDist > 0.2f)
            {
                float translatedDist = Mathf.Abs(player.position.y - ropeStartPoint.y);
                leftDist = lenth + 0.5f - translatedDist;
                rb.linearVelocityY = Mathf.Clamp(leftDist * 5f, 3f, 15f);
                line.SetPosition(0, ropeStartPoint + (lenth - leftDist) * Vector3.up);
                line.SetPosition(1, hook.position);

                await UniTask.Yield(cancellationToken);
            }
        }

        private async UniTask UnhookedRoutine(float lenth, CancellationToken cancellationToken)
        {
            var ropePainted = 0f;
            var speed = Mathf.Min((lenth - ropePainted) * 5f + 0.2f * paintSpeed, paintSpeed);

            bool isForward = true;
            while (ropePainted >= 0)
            {
                if (isForward && ropePainted >= lenth)
                    isForward = false;

                ropePainted += (isForward ? 1 : -1) * speed * Time.deltaTime;
                DrawRope(ropePainted);
                await UniTask.Yield(cancellationToken);
            }
        }

        private void DrawRope(float length)
        {
            hook.position = ropeStartPoint + Vector3.up * length;
            line.SetPosition(0, hook.position);
            line.SetPosition(1, ropeStartPoint);
        }

        private void InitializeHook()
        {
            cts?.Cancel();
            cts = new();

            line.gameObject.SetActive(true);
            rb.linearVelocity = Vector2.zero;
            controller.enabled = false;
            hook.gameObject.SetActive(true);
            hook.SetParent(null);
            ropeStartPoint = (Vector2)transform.position;
        }

        private void ResetHook()
        {
            // onResetHook();
            hook.SetParent(transform);
            hook.localPosition = hookLocalPosition;
            // isUpperDirection = true;
            controller.enabled = true;
            line.gameObject.SetActive(false);
            rb.GetComponent<Collider2D>().isTrigger = false;
        }

        // private void DrawRope()
        // {
        //     if (ropePainted < ropeLength)
        //     {
        //         var speed = Mathf.Min((ropeLength - ropePainted) * 5f + 0.2f * paintSpeed, paintSpeed);
        //         ropePainted += speed * Time.deltaTime;
        //         if (ropePainted >= ropeLength)
        //         {
        //             isHooked = false;
        //             ropePainted = ropeLength;
        //             isTransported = false;
        //             rb.GetComponent<Collider2D>().isTrigger = true;
        //             rb.linearVelocity = Vector2.zero;
        //             // hook.gameObject.SetActive(false);
        //         }
        //         hook.position = ropeStartPoint + Vector3.up * ropePainted;
        //         line.SetPosition(0, hook.position);
        //         line.SetPosition(1, ropeStartPoint);
        //     }
        // }

        // private void DrawUnhookedRope()
        // {
        //     var speed = Mathf.Min((ropeLength - ropePainted) * 5f + 0.2f * paintSpeed, paintSpeed);
        //     if (isUpperDirection)
        //     {
        //         ropePainted += speed * Time.deltaTime;
        //         if (ropePainted >= ropeLength)
        //         {
        //             isUpperDirection = false;
        //         }
        //     }
        //     else
        //     {
        //         ropePainted -= speed * Time.deltaTime;
        //         if (ropePainted <= 0)
        //         {
        //             ResetHook();
        //             return;
        //         }
        //     }
        //     hook.position = ropeStartPoint + Vector3.up * ropePainted;
        //     line.SetPosition(0, hook.position);
        //     line.SetPosition(1, ropeStartPoint);
        // }

        // private void TransportPlayer()
        // {
        //     var leftDist = ropeLength + 0.5f - Mathf.Abs(player.position.y - ropeStartPoint.y);
        //     if (leftDist > 0.1f)
        //     {
        //         rb.linearVelocityY = Mathf.Clamp(leftDist * 5f, 3f, 15f);
        //         line.SetPosition(0, ropeStartPoint + (ropeLength - leftDist) * Vector3.up);
        //         line.SetPosition(1, hook.position);
        //     }
        //     else
        //     {
        //         isTransported = true;
        //         rb.GetComponent<Collider2D>().isTrigger = false;
        //         ResetHook();
        //     }
        // }

        private bool HasHookCollision(Vector2 startPosition, out Vector2 hookedLocation)
        {
            bool isEntered = false;
            hookedLocation = startPosition;

            for (int i = 1; i < maxRopeLength; i++)
            {
                var start = startPosition + Vector2.up * i;
                var end = start + Vector2.down;

                var col = Physics2D.Linecast(start, end, groundLayer.value);
                if (col.collider != null)
                {
                    if (col.collider != null && isEntered)
                    {
                        hookedLocation = col.point;
                        return true;
                    }
                    isEntered = true;
                }
            }
            return false;
        }

        public async override UniTask Use(Vector2 from, Vector2 to, LayersSO layer, int defaultDamage)
        {
            groundLayer = layer.ground.value;
            await CastRope();
        }
    }
}
