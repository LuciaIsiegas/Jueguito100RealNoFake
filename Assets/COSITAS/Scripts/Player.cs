using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 5f;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private Vector2 movement;
    [SerializeField] HidingSystem hidingSystem;
    private bool isHiding = false;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    void Update()
    {
        InputPlayer();
        isHiding = hidingSystem.IsHiding;
        if (!isHiding)
        {
            Move();
        }
    }

    void InputPlayer()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
    }

    private void Move()
    {
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
        if (isHiding)
        {
            rb.velocity = Vector2.zero;
            return;
        }
        rb.MovePosition(rb.position + movement.normalized * speed * Time.fixedDeltaTime);
    }
}