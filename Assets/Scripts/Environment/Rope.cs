using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Rope : MonoBehaviour
{
    [Header("Rope Dimensions")]
    public float topY;
    public float bottomY;
    public float climbSpeed = 4.5f;

    [Header("Optional Top/Bottom Platforms")]
    public Vector2 topDismountOffset = new Vector2(0f, 0.75f);
    public Vector2 bottomDismountOffset = new Vector2(0f, 0.75f);

    private bool isPlayerInRange = false;
    private GameObject playerObj;
    private Rigidbody2D playerRb;
    private PlayerMovement playerMovement;
    private Collider2D playerCol;
    private float originalGravityScale = 1f;
    private bool isClimbing = false;

    private void Awake()
    {
        var col = GetComponent<BoxCollider2D>();
        col.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.gameObject.name == "Player")
        {
            isPlayerInRange = true;
            playerObj = collision.gameObject;
            playerRb = playerObj.GetComponent<Rigidbody2D>();
            playerMovement = playerObj.GetComponent<PlayerMovement>();
            playerCol = collision;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == playerObj)
        {
            if (isClimbing)
            {
                StopClimbing();
            }
            isPlayerInRange = false;
            playerObj = null;
        }
    }

    private void Update()
    {
        if (!isPlayerInRange || playerObj == null) return;

        float vInput = Input.GetAxisRaw("Vertical");
        float hInput = Input.GetAxisRaw("Horizontal");

        if (!isClimbing)
        {
            // Start climbing if player presses W (up) or S (down)
            if (Mathf.Abs(vInput) > 0.1f)
            {
                StartClimbing();
            }
        }
        else
        {
            // If climbing and player presses A/D to dismount
            if (Mathf.Abs(hInput) > 0.5f && Mathf.Abs(vInput) < 0.1f)
            {
                DismountWithHorizontalKick(hInput);
                return;
            }

            // Check if reached top
            if (playerObj.transform.position.y >= topY && vInput > 0f)
            {
                DismountAtTop();
                return;
            }

            // Check if reached bottom
            if (playerObj.transform.position.y <= bottomY && vInput < 0f)
            {
                DismountAtBottom();
                return;
            }
        }
    }

    private void FixedUpdate()
    {
        if (!isClimbing || playerRb == null) return;

        float vInput = Input.GetAxisRaw("Vertical");

        // Align X with rope center
        Vector3 pos = playerObj.transform.position;
        pos.x = Mathf.Lerp(pos.x, transform.position.x, Time.fixedDeltaTime * 15f);
        playerObj.transform.position = pos;

        // Move vertically
        playerRb.linearVelocity = new Vector2(0f, vInput * climbSpeed);
    }

    private Collider2D groundCol;
    private Collider2D platformCol;

    private void StartClimbing()
    {
        if (playerRb == null) return;
        isClimbing = true;
        originalGravityScale = playerRb.gravityScale;
        playerRb.gravityScale = 0f;
        playerRb.linearVelocity = Vector2.zero;
        if (playerMovement != null) playerMovement.enabled = false;

        // Temporarily ignore collision with Ground and Platforms so player can smoothly climb through
        if (playerCol != null)
        {
            var ground = GameObject.Find("Ground");
            if (ground != null)
            {
                groundCol = ground.GetComponent<Collider2D>();
                if (groundCol != null) Physics2D.IgnoreCollision(playerCol, groundCol, true);
            }

            var plat = GameObject.Find("Platforms");
            if (plat != null)
            {
                platformCol = plat.GetComponent<Collider2D>();
                if (platformCol != null) Physics2D.IgnoreCollision(playerCol, platformCol, true);
            }
        }
    }

    private void StopClimbing()
    {
        if (!isClimbing) return;
        isClimbing = false;
        if (playerRb != null)
        {
            playerRb.gravityScale = originalGravityScale;
        }
        if (playerMovement != null) playerMovement.enabled = true;

        if (playerCol != null)
        {
            if (groundCol != null)
            {
                Physics2D.IgnoreCollision(playerCol, groundCol, false);
                groundCol = null;
            }

            if (platformCol != null)
            {
                Physics2D.IgnoreCollision(playerCol, platformCol, false);
                platformCol = null;
            }
        }
    }

    private void DismountAtTop()
    {
        StopClimbing();
        if (playerObj != null)
        {
            playerObj.transform.position = new Vector3(transform.position.x, topY + topDismountOffset.y, playerObj.transform.position.z);
        }
    }

    private void DismountAtBottom()
    {
        StopClimbing();
        if (playerObj != null)
        {
            playerObj.transform.position = new Vector3(transform.position.x, bottomY + bottomDismountOffset.y, playerObj.transform.position.z);
        }
    }

    private void DismountWithHorizontalKick(float hDir)
    {
        StopClimbing();
        if (playerRb != null)
        {
            playerRb.linearVelocity = new Vector2(hDir * 3f, 1f);
        }
    }

    private void OnMouseDown()
    {
        // Clicking on rope automatically climbs up
        if (isPlayerInRange && !isClimbing)
        {
            StartClimbing();
        }
    }
}
