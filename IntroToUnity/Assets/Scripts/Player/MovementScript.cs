using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementScript : MonoBehaviour
{
    [Header("Player Stats")]
    public float speed = 100f;
    public float crouchingSpeed = 50f;
    float movSpeed;

    public int jumpCount = 1;
    int currJumpCount;
    public float jumpHeight = 5f;

    [Header("Player States")]
    public bool isCrouching;

    public bool playerWins = false;
    public bool playerLoses = false;
    public bool inPlay = true;
    public bool isPaused;
    public bool canDie;

    Vector2 hitboxHeight;
    Vector2 hitboxCenter;

    private SpriteRenderer animations;
    private Animator animator;
    private Rigidbody2D rb;
    private BoxCollider2D playerHitbox;

    /*Awake is called the moment an object is loaded, in this case I use it to reset player states.
    Resetting player states is important for decreasing bugs and makes it so that they player
    feels continuous*/
    private void Awake()
    {
        playerWins = false;
        playerLoses = false;
        isPaused = false;
        isCrouching = false;
        canDie = false;
        inPlay = true;

        Time.timeScale = 1f;
    }

    // Start is called before the first frame update
    void Start()
    {
        movSpeed = speed;
        currJumpCount = jumpCount;
        canDie = true;

        rb = gameObject.GetComponent<Rigidbody2D>();
        playerHitbox = gameObject.GetComponent<BoxCollider2D>();
        animations = gameObject.GetComponent<SpriteRenderer>();
        animator = gameObject.GetComponent<Animator>();

        hitboxHeight = playerHitbox.size;
        hitboxCenter = playerHitbox.offset;
    }

    // Update is called once per frame
    void Update()
    {
        if(playerWins || playerLoses)
        {
            inPlay = false;
        }
        if (inPlay)
        {
            //Moving
            float movX = Input.GetAxis("Horizontal") * movSpeed / 10;

            animator.SetFloat("Speed", Mathf.Abs(movX));

            if (movX < 0)
            {
                animations.flipX = true;
            }
            if (movX > 0)
            {
                animations.flipX = false;
            }

            rb.velocity = new Vector2(movX, rb.velocity.y);

            //Jumping
            if (currJumpCount > 0 && Input.GetButtonDown("Jump"))
            {
                rb.velocity = new Vector2(0, jumpHeight);
                currJumpCount--;
            }

            //Crouching
            if (Input.GetKey(KeyCode.LeftShift))
            {
                Crouch();

                //makes it so that you fall faster while crouching
                if (rb.velocity.y < 0)
                {
                    rb.gravityScale = 2.5f;
                }
                else
                {
                    rb.gravityScale = 1;
                }
            }
            else
            {
                playerHitbox.size = hitboxHeight;
                playerHitbox.offset = hitboxCenter;
                movSpeed = speed;
                animator.SetBool("isCrouching", false);
                isCrouching = false;
            }
        }

        //Softlock prevention (very important)
        if(gameObject.transform.position.y <= -150)
        {
            playerLoses = true;
        }
    }

    void Crouch()
    {
        playerHitbox.size = new Vector2(1, 0.5035717f);
        playerHitbox.offset = new Vector2(0, -0.2412141f);
        movSpeed = crouchingSpeed;
        isCrouching = true;
        animator.SetBool("isCrouching", true);
    }

    //Detects if the player has collided with anything
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Refreshes jump
        if (collision.gameObject.tag == "Ground" && currJumpCount <= 0)
        {
            currJumpCount = jumpCount;
        }
        //Detects Enemies
        if (collision.gameObject.tag == "Enemy" && canDie)
        {
            playerLoses = true;
        }
        //Detects if you collided with a portal, which loads the next level
        if(collision.gameObject.tag == "Portal")
        {
            playerWins = true;
            canDie = false;
            inPlay = false;

            WinEvent portal = collision.gameObject.GetComponent<WinEvent>();
            portal.PlayerWins();
        }
        Vector3 normal = collision.GetContact(0).normal;
        if(normal == Vector3.up)
        {
            animator.SetBool("isFalling", false);
            animator.SetBool("hasLanded", true);
        }
        
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        animator.SetBool("hasLanded", false);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        animator.SetBool("isFalling", true);
        animator.SetBool("hasLanded", false);
    }
}
