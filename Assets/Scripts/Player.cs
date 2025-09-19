using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveForce = 10F;
    public float jumpForce = 11F;
    private float movementX;

    public Score scoreScript;

    private Rigidbody2D myRigidbody2D;
    private Animator animator;
    private SpriteRenderer sr;

    private bool isGrounded;

    private readonly string IS_WALK_ANIMATION = "isWalking";
    private readonly string GROUND_TAG = "Ground";
    private readonly string ENEMY_TAG = "Enemy";

    public void Start()
    {
        myRigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        //settings
        myRigidbody2D.gravityScale = 2F;
        if (scoreScript == null)
        {
            scoreScript = FindFirstObjectByType<Score>();
        }
    }

    public void Update()
    {
        PlayerMoveKeyboard();
        AnimatePlayer();
    }

    public void FixedUpdate() //run every interval (on physics)
    {
        PlayerJump();
    }

    public void PlayerMoveKeyboard()
    {
        movementX = Input.GetAxis("Horizontal");
        //-1: A key, +1: D key, 0: No 
        // key GexAxis(continous) Raw(descrete values)

        Vector3 frameMovement = moveForce * Time.deltaTime * new Vector3(movementX, 0f, 0f);
        transform.position += frameMovement;
    }

    public void AnimatePlayer()
    {
        if (movementX > 0)   // goint to the right
        {
            animator.SetBool(IS_WALK_ANIMATION, true);
            sr.flipX = false;
        }
        else if (movementX < 0) // going to the left
        {
            animator.SetBool(IS_WALK_ANIMATION, true);
            sr.flipX = true;
        }
        else  // not moving
        {
            animator.SetBool(IS_WALK_ANIMATION, false);
        }
    }

    public void PlayerJump()
    {
        bool isJumpKey = Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow); //by default it is space
        bool isFallKey = Input.GetKeyDown(KeyCode.DownArrow);

        if (isJumpKey && isGrounded)
        {
            isGrounded = false;
            Vector2 force = new Vector2(0f, jumpForce);
            ForceMode2D forcemode = ForceMode2D.Impulse;
            myRigidbody2D.AddForce(force, forcemode);
        }

        if (isFallKey)
        {
            Vector2 force = new Vector2(0f, -jumpForce);
            ForceMode2D forcemode = ForceMode2D.Impulse;
            myRigidbody2D.AddForce(force, forcemode);
            isGrounded = true;
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(GROUND_TAG))
        {
            isGrounded = true;
        }

        if (collision.gameObject.CompareTag(ENEMY_TAG))
        {
            Destroy(gameObject);
            scoreScript.isGameOver = true;
            if (scoreScript.GameOverText != null)
                scoreScript.GameOverText.gameObject.SetActive(true);
            else
                Debug.LogWarning("GameOverText not assigned in Score!");
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(ENEMY_TAG))
        {
            Destroy(gameObject);
        }
    }
}
