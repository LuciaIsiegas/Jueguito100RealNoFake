using UnityEngine;
using System.Collections.Generic;

public class Enemy : MonoBehaviour
{
    [Header("Waypoints")]
    public List<Transform> waypoints = new List<Transform>();

    [Header("Velocidades")]
    public float patrolSpeed = 2f;
    public float chaseSpeed = 3.5f;
    private float lostPlayerTimer = 0f;
    public float lostPlayerDelay = 3f;

    [Header("Detección")]
    public string playerTag = "Player";

    private int currentWaypoint = 0;
    private int direction = 1;
    private Transform playerTarget;
    private bool chasingPlayer = false;
    HidingSystem playerHiding;
    HealthUI healthUI;

    private void Start()
    {
        healthUI = FindObjectOfType<HealthUI>();
    }

    void Update()
    {
        if (playerHiding != null && playerHiding.IsHiding)
        {
            chasingPlayer = false;
            lostPlayerTimer += Time.deltaTime;

            if (lostPlayerTimer >= lostPlayerDelay)
            {
                lostPlayerTimer = 0f;
                currentWaypoint = GetClosestWaypointIndex();
                UpdateDirection();
                Patrol();
            }
            return;
        }

        lostPlayerTimer = 0f;  // resetea el timer si el player no está escondido

        if (chasingPlayer && playerTarget != null)
            ChasePlayer();
        else
            Patrol();
    }

    void Patrol()
    {
        if (waypoints.Count < 2) return;

        transform.position = Vector2.MoveTowards(
            transform.position,
            waypoints[currentWaypoint].position,
            patrolSpeed * Time.deltaTime
        );

        if (playerHiding != null && playerHiding.IsHiding)
        {
            return;
        }

            if (Vector2.Distance(transform.position, waypoints[currentWaypoint].position) < 0.05f)
        {
            if (currentWaypoint == waypoints.Count - 1)
                direction = -1;
            else if (currentWaypoint == 0)
                direction = 1;

            currentWaypoint += direction;
        }

        FlipSprite(waypoints[currentWaypoint].position);
    }

    // ---------- PERSECUCIÓN ----------
    void ChasePlayer()
    {
        transform.position = Vector2.MoveTowards(
            transform.position,
            playerTarget.position,
            chaseSpeed * Time.deltaTime
        );

        FlipSprite(playerTarget.position);
    }

    // ---------- DETECCIÓN ----------
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            playerTarget = other.transform;
            playerHiding = playerTarget.GetComponentInChildren<HidingSystem>();

            // Solo perseguir si no está escondido
            if (playerHiding == null || !playerHiding.IsHiding)
                chasingPlayer = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            playerTarget = null;
            chasingPlayer = false;

            // 🔥 volver al waypoint más cercano
            currentWaypoint = GetClosestWaypointIndex();
            UpdateDirection();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(playerTag))
        {
            // No hacer daño si está escondido
            if (playerHiding != null && playerHiding.IsHiding) return;
            healthUI.TakeDamage(1);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        
    }

    // ---------- WAYPOINT MÁS CERCANO ----------
    int GetClosestWaypointIndex()
    {
        int closestIndex = 0;
        float closestDistance = Mathf.Infinity;

        for (int i = 0; i < waypoints.Count; i++)
        {
            float dist = Vector2.Distance(transform.position, waypoints[i].position);
            if (dist < closestDistance)
            {
                closestDistance = dist;
                closestIndex = i;
            }
        }

        return closestIndex;
    }

    // ---------- AJUSTAR DIRECCIÓN ----------
    void UpdateDirection()
    {
        if (currentWaypoint == 0)
            direction = 1;
        else if (currentWaypoint == waypoints.Count - 1)
            direction = -1;
    }

    // ---------- GIRO DEL SPRITE ----------
    void FlipSprite(Vector3 target)
    {
        Vector3 scale = transform.localScale;
        float absX = Mathf.Abs(scale.x); // guarda el valor original sin el signo

        if (target.x > transform.position.x)
            transform.localScale = new Vector3(absX, scale.y, scale.z);
        else
            transform.localScale = new Vector3(-absX, scale.y, scale.z);
    }
}