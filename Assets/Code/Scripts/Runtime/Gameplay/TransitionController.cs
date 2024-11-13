using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TransitionController : MonoBehaviour
{
    public float speed = 3f;
    public float gravityForce = 9.8f;
    public float jumpPower;
    public Image powerGage;
    public float gageIncrease = 10;
    private float jumpForce;

    private PCInput input;
    [SerializeField] private float groundRayDistance;
    [SerializeField] private LayerMask groundLayer;
    private bool isGround;
    public int targetFrameRate;
    private Rigidbody2D rigid;
    private bool isOnAir;
    private Vector2 velocityOnJump;
    private bool jumpStart;
    private Camera mainCam;

    void Start()
    {
        Application.targetFrameRate = targetFrameRate;
        input = new PCInput();
        input.Player.Enable();

        rigid = GetComponent<Rigidbody2D>();

        input.Player.Jump.performed += OnPressJump;
        input.Player.Jump.canceled += OnStartJump;
        isGround = true;
        isOnAir = false;

        mainCam = Camera.main;
    }

    private void OnDestroy()
    {
        input.Player.Disable();
        input = null;
    }

    void Update()
    {
        isOnAir = !IsOnGround();
        isGround = !isOnAir;
        if (!isOnAir)
        {
            var velocity = input.Player.Transit.ReadValue<Vector2>() * speed;
            rigid.linearVelocityX = velocity.x;
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
        Vector3Int playerPos = new Vector3Int(Mathf.FloorToInt(originPos.x), Mathf.FloorToInt(originPos.y + 4), 0) / 8;
        playerPos.z = -10;
        mainCam.transform.position = playerPos * 8;
    }

    private void OnPressJump(InputAction.CallbackContext context)
    {
        if (isGround)
        {
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
            Debug.Log("On Ground");
            isOnAir = false;
            isGround = true;
        }
        else
        {
            isOnAir = true;
            rigid.linearVelocityX = -velocityOnJump.x;
            Debug.Log("Next To Wall");
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
