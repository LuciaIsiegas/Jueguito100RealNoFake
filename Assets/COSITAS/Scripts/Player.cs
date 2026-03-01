using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 5f;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private Vector2 movement;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        bool isMoving = movement != Vector2.zero;
        animator.SetBool("IsMoving", isMoving);

        if (!isMoving) return;

        // PRIORIDAD: vertical primero
        if (movement.y > 0)
        {
            animator.Play("Walkup");
        }
        else if (movement.y < 0)
        {
            animator.Play("Walkdown");
        }
        else
        {
            animator.Play("WalkSide");

            // Flip del sprite
            if (movement.x < 0)
                spriteRenderer.flipX = true;
            else if (movement.x > 0)
                spriteRenderer.flipX = false;
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement.normalized * speed * Time.fixedDeltaTime);
    }
}