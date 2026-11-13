using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public int coin;
    public int Health = 100;
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;
    public Image healthImage;

    [Header("Shooting")]
    public GameObject bulletPrefab;
    public float fireRate;

    private float fireTimer;

    [Header("Invincibility")]
    public float IFrameDuration = 1f;
    public bool isInvincible;

    [Header("wall sliding")]
    public float wallCheckDistance = -22f;
    public float WallSlideSpeed = 2f;

    private bool isTouchingWall;
    private bool isWallSliding;

    private Rigidbody2D rb;
    private bool isGrounded;

    private Animator animator;

    private SpriteRenderer spriteRenderer;
    public int extraJumpValue = 1;
    private int extraJumps;

    public float coyoteTime = 0.2f;
    private float coyoteTimeCounter;

    public float JumpBufferTime = 0.15f;
    public float JumpBufferCounter;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        extraJumps = extraJumpValue;

        if (Checkpoint.savedPosition != Vector2.zero)
        {
            transform.position = Checkpoint.savedPosition;
        }
    }


    void Update()
    {
        float moveInput = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        if(rb.linearVelocityX != 0)
        {
            if (rb.linearVelocityX > 0)
            {
                spriteRenderer.flipX = false;
            }
            else
            {
                spriteRenderer.flipX = true;
            }
        }

        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
            extraJumps = extraJumpValue;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            JumpBufferCounter = JumpBufferTime;
        }
        else
        {
            JumpBufferCounter = Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Space)/*JumpBufferCounter > 0f*/)
        {
            if (isWallSliding)
            {
                float direction = spriteRenderer.flipX ? 1 : -1;

                rb.linearVelocity = new Vector2(direction * 8f, jumpForce);

                spriteRenderer.flipX = !spriteRenderer.flipX;
                JumpBufferCounter = 0f;
            }
            else if (coyoteTimeCounter > 0f)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                coyoteTimeCounter = 0f;
                JumpBufferCounter = 0f;
            }
            else if (extraJumps > 0)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                extraJumps--;
                JumpBufferCounter = 0f;
            }
        }

        SetAnimation(moveInput);

        healthImage.fillAmount = Health / 100f;

        if(rb.linearVelocityY < 0)
        {
            rb.gravityScale = 2.5f;
        }
        else
        {
            rb.gravityScale = 1.2f;
        }

        if (transform.position.y < -29.5)
        {
            Die();
        }

            HandleShooting();
        HandlewallSlide(moveInput);
    }

    private void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        isTouchingWall = Physics2D.Raycast(transform.position, spriteRenderer.flipX ? Vector2.left : Vector2.right);
    }

    private void SetAnimation(float moveInput)
    {
        if (isGrounded)
        {
            if (moveInput == 0)
            {
                animator.Play("Player_Idle");
            }
            else
            {
                animator.Play("Player_Run");
            }
        }
        else
        {
            if (rb.linearVelocityY > 0)
            {
                animator.Play("Player_Jump");
            }
            else
            {
                animator.Play("Player_Fall");
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Damage")
        {
            if (isInvincible) return;

            Health -= 25;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            StartCoroutine(BlinkRed());

            StartCoroutine(InvincibilityFrames());

            if (Health <= 0)
            {
                Die();
            }
        }
    }

    

    private IEnumerator BlinkRed()
    {
        spriteRenderer.color = new Color(Color.red.r, Color.red.g, Color.red.g, spriteRenderer.color.a);
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = new Color(Color.white.r, Color.white.g, Color.white.g, spriteRenderer.color.a);

    }

    private void Die()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    private IEnumerator InvincibilityFrames()
    {
        isInvincible = true;

        float elapsed = 0f;

        while(elapsed < IFrameDuration)
        {
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0.3f);
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1);
            yield return new WaitForSeconds(0.1f);

            elapsed += 0.2f;
        }

        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1);
        isInvincible = false;
    }

    private void HandlewallSlide(float moveinput)
    {
        if(isTouchingWall && isGrounded && moveinput != 0 && rb.linearVelocityY < 0)
        {
            isWallSliding = true;
            rb.linearVelocityY = -WallSlideSpeed;
        }
        else
        {
            isWallSliding = false;
        }
    }
     
    private void HandleShooting()
    {
        fireTimer -= Time.deltaTime;

        if(Input.GetMouseButton(0) && fireTimer <= 0f)
        {
            Shoot();
            fireTimer = fireRate;
        }
    }

    private void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

        Bullet bulletScript = bullet.GetComponent<Bullet>();
        
        if (spriteRenderer.flipX)
        {
            bulletScript.SetDirection(Vector2.left);
        }
        else
        {
            bulletScript.SetDirection(Vector2.right);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Bullet" && transform.parent.gameObject != null)
        {
            Destroy(transform.parent.gameObject);
        }
    }
}
