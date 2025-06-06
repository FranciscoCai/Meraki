using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Constructor_RunBehaviour : StateMachineBehaviour
{
    private NavMeshAgent agent;
    private AgentContrucotr agentController;
    private Vector3 rayOrigin;

    public float speedAgent = 10f; // Velocidad al correr
    private float originalSpeed;  // Velocidad original

   

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Obtener el componente AgentContructor para acceder a los waypoints
        agentController = animator.GetComponent<AgentContrucotr>();
        if (agentController == null)
        {
            Debug.LogError("AgentContructor no encontrado en el NPC.");
            return;
        }

        agent = animator.GetComponent<NavMeshAgent>();

        if (agent == null)
        {
            Debug.LogError("NavMeshAgent no encontrado en el NPC.");
            return;
        }

        if (agentController.waypoints == null || agentController.waypoints.Length == 0)
        {
            Debug.LogWarning("No se han asignado waypoints para el Constructor.");
            return;
        }

        // Guardar la velocidad original y establecer la nueva velocidad
        originalSpeed = agent.speed;
        agent.speed = speedAgent;

        
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
                if (agentController.waypoints[agentController.currentWaypointIndex].gameObject.transform.parent == null ||
                    agentController.waypoints[agentController.currentWaypointIndex].gameObject.transform.parent.GetComponent<DataTurtle>() == null ||
                    agentController.waypoints[agentController.currentWaypointIndex].gameObject.transform.parent.GetComponent<DataTurtle>().isStoped == false)
                {
                    return;
                }
                agentController.currentWaypointIndex = (agentController.currentWaypointIndex + 1) % agentController.waypoints.Length;

                agent.enabled = false;
                animator.SetTrigger("ToTurtleWalk");
            }
            else
            {
                agent.SetDestination(agentController.waypoints[agentController.currentWaypointIndex].position);
            }
        }


      

        // Detectar al Dino en un radio de 10 metros
        RaycastHit[] hits = Physics.SphereCastAll(agentController.transform.position, 10f, Vector3.up, 0.01f);
        if (hits != null && hits.Length > 0)
        {
            bool hasDino = false;
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].collider.CompareTag("Dino"))
                {
                    hasDino = true;
                    break;
                }
            }
            if (!hasDino)
            {
                animator.SetBool("ToRun", false); // Cambiar al estado 'ToWalk'
            }
        }

        RaycastHit[] KO = Physics.SphereCastAll(agentController.transform.position, 3f, Vector3.forward, 0.01f); // 3f es scale y 0.01 es punto central
        if (KO != null && KO.Length > 0)
        {
           
            for (int i = 0; i < KO.Length; i++)
            {
                if (KO[i].collider.CompareTag("Dino"))
                {
                    animator.SetTrigger("ToKO"); // Cambiar al estado 'ToKO'
                    break;
                }
            }
            
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Restaurar la velocidad original al salir del estado
        agent.speed = originalSpeed;

        if (agent.enabled)
        {
            agent.ResetPath();
        }
    }
}