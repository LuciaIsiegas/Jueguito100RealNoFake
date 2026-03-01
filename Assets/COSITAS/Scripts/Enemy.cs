using UnityEngine;
using System.Collections.Generic;

public class Enemy : MonoBehaviour
{
    [Header("Waypoints")]
    public List<Transform> waypoints = new List<Transform>();

    [Header("Velocidades")]
    public float patrolSpeed = 2f;
    public float chaseSpeed = 3.5f;

    [Header("Detección")]
    public string playerTag = "Player";

    private int currentWaypoint = 0;
    private int direction = 1;
    private Transform playerTarget;
    private bool chasingPlayer = false;
    HidingSystem playerHiding;

    private void Start()
    {
        playerHiding = playerTarget.GetComponent<HidingSystem>();
    }

    void Update()
    {
        if (chasingPlayer && playerTarget != null)
            ChasePlayer();
        else
            Patrol();

    }

    // ---------- PATRULLA ----------
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
        if (target.x > transform.position.x)
            transform.localScale = new Vector3(1, 1, 1);
        else
            transform.localScale = new Vector3(-1, 1, 1);
    }
}