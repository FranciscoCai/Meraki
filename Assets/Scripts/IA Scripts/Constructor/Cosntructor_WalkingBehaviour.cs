using UnityEngine;
using UnityEngine.AI;

public class Cosntructor_WalkingBehaviour : StateMachineBehaviour
{
    private NavMeshAgent agent;
    private AgentContrucotr agentController;
    private Vector3 rayOrigin;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Obtén el componente AgentContructor para acceder a los waypoints
        agentController = animator.GetComponent<AgentContrucotr>();
        agentController.c_ActualConstructorEfect = ConstructorEfect.Move;
        if (agentController == null)
        {
            Debug.LogError("AgentContructor no encontrado en el NPC.");
            return;
        }

        agent = animator.GetComponent<NavMeshAgent>();

        if (agentController.waypoints == null || agentController.waypoints.Length == 0)
        {
            Debug.LogWarning("No se han asignado waypoints para el Constructor.");
            return;
        }

    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(agentController.waypoints[agentController.currentWaypointIndex].position);
        // Actualiza la posición del raycast antes de dibujarlo
        rayOrigin = animator.transform.position + new Vector3(0, 0.5f, 0);
        Debug.DrawRay(rayOrigin, animator.transform.TransformDirection(Vector3.forward) * 10, Color.red);

        // Mover al siguiente waypoint si llega al actual
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            if (agentController.currentWaypointIndex % 2 == 0)
            {
                if (agentController.waypoints[agentController.currentWaypointIndex].gameObject.transform.parent == null)
                {
                    return;
                }
                if (agentController.waypoints[agentController.currentWaypointIndex].gameObject.transform.parent.GetComponent<DataTurtle>().isStoped == false)
                { return; }
                agentController.currentWaypointIndex = (agentController.currentWaypointIndex + 1) % agentController.waypoints.Length;
                agent.enabled = false; // desactiva el navMesh Agent del cosntructor
                animator.SetTrigger("ToTurtleWalk");
            }
            else
            {
                agentController.currentWaypointIndex = (agentController.currentWaypointIndex + 1) % agentController.waypoints.Length;
                agent.SetDestination(agentController.waypoints[agentController.currentWaypointIndex].position);
            }
        }
        // Detectar al Dino en un radio de 10 metros
        Collider[] hits = Physics.OverlapSphere(agentController.transform.position, 10f);

        if (hits.Length > 0) // Si se detecta algo en el radio
        {
            foreach (Collider hit in hits)
            {
                if (hit.CompareTag("Dino"))
                {
                    Debug.Log("Dino detectado a menos de 10 metros. Cambiando al estado ToRun.");
                    animator.SetBool("ToRun", true); // Cambia el estado a ToRun
                    return; // Salimos del método para evitar iteraciones innecesarias
                }
            }
        }

        RaycastHit[] KO = Physics.SphereCastAll(agentController.transform.position, 3f, Vector3.forward, 0.01f); // 3f es scale y 0.01 es punto central
        if (KO != null && KO.Length > 0)
        {

            for (int i = 0; i < KO.Length; i++)
            {
                if (KO[i].collider.CompareTag("Dino"))
                {
                    animator.SetTrigger("ToKO"); // Cambiar al estado 'ToWalk'
                    break;
                }
            }

        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Comprueba si el NavMeshAgent est?habilitado (si el agente puede moverse)
        if (agent.enabled)
        {
            // Si est?habilitado, limpia la ruta del agente y detiene su movimiento
            agent.ResetPath();
        }
    }
}
