using UnityEngine;
using UnityEngine.AI;

public class Constructor_IsWalking : MonoBehaviour
{
    public Transform[] waypoints; // Puntos de ruta
    private int currentWaypointIndex = 0;
    private NavMeshAgent agent;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (waypoints.Length > 0)
        {
            agent.SetDestination(waypoints[currentWaypointIndex].position);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Si el agente ha llegado al waypoint actual
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            // Actualiza al siguiente waypoint
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
            agent.SetDestination(waypoints[currentWaypointIndex].position);
        }
    }
}
