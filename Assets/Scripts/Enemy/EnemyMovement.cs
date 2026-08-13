using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 2.0f;
    [SerializeField] private float minIdleTime = 1.0f;
    [SerializeField] private float maxIdleTime = 3.0f;
    [SerializeField] private float moveDuration = 2.0f;

    private Rigidbody2D rb;
    private EnemyHealth enemyHealth;
    private Animator animator;

    private int moveDirection = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        enemyHealth = GetComponent<EnemyHealth>();
        animator = GetComponentInChildren<Animator>();

        StartCoroutine(WanderRoutine());
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveDirection * moveSpeed, rb.linearVelocity.y);

        if (animator != null)
        {
            animator.SetBool("isMoving", moveDirection != 0);
        }   
    }

    private IEnumerator WanderRoutine()
    {
        while (enemyHealth != null)
        {
            moveDirection = 0;

            float idleTime = Random.Range(minIdleTime, maxIdleTime);

            yield return new WaitForSeconds(idleTime);

            moveDirection = Random.value > 0.5f ? 1 : -1;

            FlipSprite();

            yield return new WaitForSeconds(moveDuration);
        }
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    private void FlipSprite()
    {
        if (moveDirection > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (moveDirection < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }
}
