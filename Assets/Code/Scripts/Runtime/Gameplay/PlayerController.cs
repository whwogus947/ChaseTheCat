using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed = 3f;
    public float gravityForce = 9.8f;
    public float jumpPower;

    private PCInput input;
    [SerializeField] private float groundRayDistance;
    [SerializeField] private LayerMask groundLayer;
    private float jumpForce;
    private bool isGround;
    public int targetFrameRate;

    void Start()
    {
        Application.targetFrameRate = targetFrameRate;
        input = new PCInput();
        input.Player.Enable();
        Debug.Log(input);
        Debug.Log(input.Player);

        input.Player.Jump.performed += OnPressJump;
        isGround = true;
    }

    private void OnPressJump(InputAction.CallbackContext context)
    {
        if (isGround)
        {
            jumpForce = jumpPower;
            isGround = false;
        }
    }

    private void OnDestroy()
    {
        input.Player.Disable();
        input = null;
    }

    void Update()
    {
        var value = input.Player.Transit.ReadValue<Vector2>();
        var velocity = speed * Time.deltaTime * value.x * Vector2.right + jumpForce * Time.deltaTime * Vector2.up;


        var clampedX = GetClampedVelocity(Vector2.right, velocity.x);
        var clampedY = GetClampedVelocity(Vector2.up, velocity.y);
        var deltaPosition = clampedX + clampedY;
        transform.position += (Vector3)deltaPosition;

        if (IsOnGround())
        {
            jumpForce = 0;
            isGround = true;
        }
        else
        {
            jumpForce -= gravityForce * Time.deltaTime;
        }
    }

    private Vector2 GetClampedVelocity(Vector2 direction, float power)
    {
        direction = power < 0 ? direction * -1 : direction;
        power = Mathf.Abs(power);
        // var rayHit = Physics2D.CircleCast(transform.position, 0.3f, direction, 5f, groundLayer);
        var origin = transform.position + (Vector3)direction * 0.51f;
        origin.y = transform.position.y - 0.49f;
        var rayHit = Physics2D.Raycast(origin, direction, 5f, groundLayer);
        Debug.DrawRay(origin, direction, Color.yellow);
        if (rayHit.collider != null)
        {
            // power = Mathf.Min(power, rayHit.distance - 0.2f);
            power = Mathf.Min(power, rayHit.distance - 0.01f);
        }
        return direction * power;
    }

    private bool IsOnGround()
    {
        int count = 5;
        var position = transform.position;
        position.y -= 0.31f;
        var originLeft = position - Vector3.right * 0.5f;
        var originRight = position + Vector3.right * 0.5f;
        float gap = (originRight - originLeft).x / count;

        for (int i = 0; i < count + 1; i++)
        {
            var offset = originLeft + Vector3.right * gap * i;
            if (GroundCheck(offset))
                return true;
        }
        return false;
    }

    private bool GroundCheck(Vector2 offset)
    {
        var rayHit = Physics2D.Raycast(offset, Vector2.down, 0.2f, groundLayer);
        Debug.DrawRay(offset, Vector2.down, Color.yellow);
        if (rayHit.collider != null)
        {
            return true;
        }
        return false;
    }
}
