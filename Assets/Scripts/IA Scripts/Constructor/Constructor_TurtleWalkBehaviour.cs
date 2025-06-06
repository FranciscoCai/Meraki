using UnityEngine;
using UnityEngine.AI;

public class Constructor_TurtleWalkBehaviour : StateMachineBehaviour
{
    private Transform[] waypoints; // Puntos de ruta
    private int currentWaypointIndex;
    private NavMeshAgent agent;
    private AgentContrucotr agentController;
    public float speedAgent = 5f; // Velocidad al correr
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agentController = animator.GetComponent<AgentContrucotr>();
        agentController.c_ActualConstructorEfect = ConstructorEfect.TurtleWalk;
        if (agentController == null)
        {
            return;
        }
        waypoints = agentController.waypoints;
        currentWaypointIndex = agentController.currentWaypointIndex;
        agent = animator.GetComponent<NavMeshAgent>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (waypoints[currentWaypointIndex].position != null)
        {
            // Calcula el nuevo punto hacia el que moverse
            Vector3 newPosition = Vector3.MoveTowards(animator.gameObject.transform.position, new Vector3(waypoints[currentWaypointIndex].position.x,animator.gameObject.transform.position.y, waypoints[currentWaypointIndex].position.z), speedAgent * Time.deltaTime);
            animator.gameObject.transform.position = newPosition;
            if(animator.gameObject.transform.position == new Vector3(waypoints[currentWaypointIndex].position.x, animator.gameObject.transform.position.y, waypoints[currentWaypointIndex].position.z))
            {
                currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
                agentController.currentWaypointIndex = currentWaypointIndex;
                agent.enabled = true;
                animator.SetTrigger("ToMove"); // Vuelve al estado Walk
            }
        }
    }

    //OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }

   
}
