using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    // --- Serialize Fields ---------------------------------------------------------------
    [Header("Movement")]
    [SerializeField] float moveSpeed = 6f;
    [SerializeField] float jumpForce = 18f;

    [Header("Water Movement")]
    [SerializeField] float waterGravity = 0.2f;
    [SerializeField] float waterDrag = 10f;
    [SerializeField] float waterSpeed = 4f;

    [Header("Ground Check")]
    [SerializeField] Transform groundCheck;
    [SerializeField] float groundCheckRadius = 0.15f;
    [SerializeField] LayerMask groundLayer;

    [Header("Jump Feel")]
    [SerializeField] float fallMultiplier = 2.5f;
    [SerializeField] float lowJumpMultiplier = 2f;


    // --- Private Variables ---------------------------------------------------------------
    Rigidbody2D rb;
    Animator animator;
    SpriteRenderer spriteRenderer;

    private float gravityScale;
    private float linearDrag;
    private bool isGrounded;
    private bool inWater;
    private bool isAlive = true;
    private float horizontalInput;


    // --- Lifecycle ---------------------------------------------------------------
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();   

        gravityScale = rb.gravityScale;
        linearDrag = rb.linearDamping;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAlive) return;

        ReadInput();
        CheckGrounded();  
        HandleJump();
        FlipSprite();
        UpdateAnimator();
    }

    void FixedUpdate()
    {
        if (!isAlive) return;
        Move();
        ApplyBetterJumpPhysics();
    }

    // --- Input ---------------------------------------------------------------
    void ReadInput()
    {
        horizontalInput = 0f;

        if (Keyboard.current.aKey.isPressed)
            horizontalInput = -1f;
        else if (Keyboard.current.dKey.isPressed)
            horizontalInput = 1f;
    }

    // --- Movement ---------------------------------------------------------------
    void Move()
    {
        float currentSpeed = inWater ? waterSpeed : moveSpeed;
        rb.linearVelocity = new Vector2(horizontalInput * currentSpeed, rb.linearVelocity.y);
    }

    // --- Ground Check ---------------------------------------------------------------
    void CheckGrounded()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    // --- Water Movement -----------------------------------------------------------------
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Water"))
        {
            inWater = true;
            rb.gravityScale = waterGravity;
            rb.linearDamping = waterDrag;
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Water"))
        {
            inWater = false;
            rb.gravityScale = gravityScale;
            rb.linearDamping = linearDrag;
        }
    }

    // --- Jump ---------------------------------------------------------------
    void HandleJump()
    {
        if ((Keyboard.current.spaceKey.wasPressedThisFrame || Keyboard.current.wKey.wasPressedThisFrame) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    void ApplyBetterJumpPhysics()
    {
        if (inWater) return;
        if (rb.linearVelocity.y < 0)
        {
            // Falling — apply extra gravity
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }
        else if (rb.linearVelocity.y > 0 && !(Keyboard.current.spaceKey.isPressed || Keyboard.current.wKey.isPressed))
        {
            // Released jump early — cut height
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.fixedDeltaTime;
        }
    }

    // --- Sprite Flip ---------------------------------------------------------------
    void FlipSprite()
    {
        if (horizontalInput > 0)
            spriteRenderer.flipX = false;
        else if (horizontalInput < 0)
            spriteRenderer.flipX = true;
    }

    // --- Animator ---------------------------------------------------------------
    void UpdateAnimator()
    {
        bool isRunning = Mathf.Abs(horizontalInput) > Mathf.Epsilon;
        animator.SetBool("isRunning", isRunning);
        animator.SetBool("isGrounded", isGrounded);
        animator.SetFloat("yVelocity", rb.linearVelocity.y);
    }
    
    // --- Public API ---------------------------------------------------------------
    public void onDeath()
    {
        isAlive = false;
        animator.SetTrigger("die");
        rb.linearVelocity = Vector2.zero;
    }
}
