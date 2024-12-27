using UnityEngine;
using UnityEngine.Events;

namespace Com2usGameDev
{
    public class HookController : MonoBehaviour
    {
        public LineRenderer line;
        public float paintSpeed = 1f;
        public Transform hook;
        public LayerMask groundLayer;
        public int maxRopeLength = 15;

        private bool isHooked;
        private bool isUnhooked = false;
        private float ropeLength;
        private float ropePainted;
        private Vector3 ropeStartPoint;
        private Transform player;
        private bool isTransported = true;
        private Rigidbody2D rb;
        private Vector2 hookLocalPosition;
        private bool isUpperDirection = true;
        private UnityAction onResetHook;
        private PlayerController userInputHandler;
        
        void Start()
        {
            player = transform.root;
            rb = player.GetComponent<Rigidbody2D>();
            userInputHandler = player.GetComponent<PlayerController>();
            hookLocalPosition = hook.localPosition;
            line.gameObject.SetActive(false);
        }

        void Update()
        {
            if (isHooked)
            {
                DrawRope();
            }
            else if (!isTransported)
            {
                TransportPlayer();
            }

            if (isUnhooked)
            {
                DrawUnhookedRope();
            }
        }

        public void CastRope(UnityAction onReset)
        {
            line.gameObject.SetActive(true);
            onResetHook = onReset;
            rb.linearVelocity = Vector2.zero;
            userInputHandler.enabled = false;
            hook.gameObject.SetActive(true);
            hook.SetParent(null);
            isHooked = false;
            isUnhooked = false;
            var start = transform.position;
            ropeStartPoint = start;
            if (TryCastHook(start, out Vector2 end))
            {
                isHooked = true;
                ropeLength = Vector2.Distance(start, end);
            }
            else
            {
                ropeLength = maxRopeLength;
                isUnhooked = true;
                isUpperDirection = true;
            }
        }

        private void ResetHook()
        {
            onResetHook();
            hook.SetParent(transform);
            hook.localPosition = hookLocalPosition;
            isTransported = true;
            isHooked = false;
            isUnhooked = false;
            isUpperDirection = true;
            userInputHandler.enabled = true;
            line.gameObject.SetActive(false);
        }

        private void DrawRope()
        {
            if (ropePainted < ropeLength)
            {
                var speed = Mathf.Min((ropeLength - ropePainted) * 5f + 0.2f * paintSpeed, paintSpeed);
                ropePainted += speed * Time.deltaTime;
                if (ropePainted >= ropeLength)
                {
                    isHooked = false;
                    ropePainted = ropeLength;
                    isTransported = false;
                    rb.GetComponent<Collider2D>().isTrigger = true;
                    rb.linearVelocity = Vector2.zero;
                    // hook.gameObject.SetActive(false);
                }
                hook.position = ropeStartPoint + Vector3.up * ropePainted;
                line.SetPosition(0, hook.position);
                line.SetPosition(1, ropeStartPoint);
            }
        }

        private void DrawUnhookedRope()
        {
            var speed = Mathf.Min((ropeLength - ropePainted) * 5f + 0.2f * paintSpeed, paintSpeed);
            if (isUpperDirection)
            {
                ropePainted += speed * Time.deltaTime;
                if (ropePainted >= ropeLength)
                {
                    isUpperDirection = false;
                }
            }
            else
            {
                ropePainted -= speed * Time.deltaTime;
                if (ropePainted <= 0)
                {
                    ResetHook();
                    return;
                }
            }
            hook.position = ropeStartPoint + Vector3.up * ropePainted;
            line.SetPosition(0, hook.position);
            line.SetPosition(1, ropeStartPoint);
        }

        private void TransportPlayer()
        {
            var leftDist = ropeLength + 0.5f - Mathf.Abs(player.position.y - ropeStartPoint.y);
            if (leftDist > 0.1f)
            {
                rb.linearVelocityY = Mathf.Clamp(leftDist * 5f, 3f, 15f);
                line.SetPosition(0, ropeStartPoint + (ropeLength - leftDist) * Vector3.up);
                line.SetPosition(1, hook.position);
            }
            else
            {
                isTransported = true;
                rb.GetComponent<Collider2D>().isTrigger = false;
                ResetHook();
            }
        }

        private bool TryCastHook(Vector2 startPosition, out Vector2 hookedLocation)
        {
            bool isEntered = false;
            hookedLocation = startPosition;
            ropePainted = 0f;
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
    }
}
