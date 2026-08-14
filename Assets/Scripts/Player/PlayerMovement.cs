using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5.0f;

    public bool isAuto = false;

    private Rigidbody2D rb;
    private PlayerAttack playerAttack;
    private Animator animator;

    private float moveInput;
    private bool isFacingRight = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerAttack = GetComponent<PlayerAttack>();
        animator = GetComponentInChildren<Animator>();

        if (!string.IsNullOrEmpty(Portal.targetSpawnName))
        {
            GameObject spawnPoint = GameObject.Find(Portal.targetSpawnName);

            if (spawnPoint != null)
            {
                transform.position = spawnPoint.transform.position;
            }

            Portal.targetSpawnName = "";
        }
    }

    // Update is called once per frame
    private void Update()
    {
        bool isAttacking = false;

        if (animator != null)
        {
            bool currentIsAttack = animator.GetCurrentAnimatorStateInfo(0).IsName("Attack");
            bool nextIsAttack = animator.GetNextAnimatorStateInfo(0).IsName("Attack");

            isAttacking = currentIsAttack || nextIsAttack;
        }

        if (isAttacking)
        {
            moveInput = 0f;
        }
        else if (!isAuto)
        {
            moveInput = Input.GetAxisRaw("Horizontal");
        }
        else
        {
            AutoMoveLogic();
        }

        CheckFlip();
    }

    private void AutoMoveLogic()
    {
        if (playerAttack == null || playerAttack.currentTarget == null)
        {
            moveInput = 0f;
            return;
        }

        float distance = Vector2.Distance(transform.position, playerAttack.currentTarget.transform.position);

        if (distance > playerAttack.attackRange - 0.1f)
        {
            if (playerAttack.currentTarget.transform.position.x > transform.position.x)
            {
                moveInput = 1f; 
            }
            else
            {
                moveInput = -1f; 
            }
        }
        else
        {
            moveInput = 0f;
        }
    }
    private void FixedUpdate()
    {
        bool isAttacking = false;
        if (animator != null)
        {
            isAttacking = animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") || animator.GetNextAnimatorStateInfo(0).IsName("Attack");
        }

        if (isAttacking)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        }
        else
        {
            rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
        }

        if (animator != null)
        {
            animator.SetBool("isWalking", moveInput != 0);
        }
    }

    private void CheckFlip()
    {
        if (moveInput > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (moveInput < 0 && isFacingRight)
        {
            Flip();
        }
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;

        Vector3 localScale = transform.localScale;

        localScale.x *= -1f;

        transform.localScale = localScale;
    }
}
