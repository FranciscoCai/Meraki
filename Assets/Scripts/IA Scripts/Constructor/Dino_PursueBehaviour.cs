using UnityEngine;
using UnityEngine.AI;

public class Dino_PursueBehaviour : StateMachineBehaviour
{
    
    
    private NavMeshAgent agent;
    private Vector3 rayOrigin;


    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Obtén el componente AgentContructor para acceder a los waypoints

        agent = animator.GetComponent<NavMeshAgent>();



    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        agent.SetDestination(animator.gameObject.GetComponent<Dino_Agent>().Constructor.transform.position);
        // Actualiza la posición del raycast antes de dibujarlo
        rayOrigin = animator.transform.position + new Vector3(0, 0.5f, 0);
        Debug.DrawRay(rayOrigin, animator.transform.TransformDirection(Vector3.forward) * 10, Color.red);

        RaycastHit[] hits = Physics.SphereCastAll(animator.transform.position, 2f, Vector3.up, 0.01f);
        if (hits != null && hits.Length > 0)
        {
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].collider.CompareTag("Constructor"))
                {
                    Debug.Log("Constructor ha muerto");
                }
            }
        }
      
    }

}

