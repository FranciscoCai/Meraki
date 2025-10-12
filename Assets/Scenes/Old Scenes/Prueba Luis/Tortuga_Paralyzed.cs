using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Tortuga_Paralyzed : StateMachineBehaviour
{
    private NavMeshAgent agent;
    float time;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent = animator.GetComponent<NavMeshAgent>();
        agent.isStopped = true;
    }
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        time += Time.deltaTime;
        if (time >= 1)
        {
            time = 0;

            animator.SetTrigger("ToNervous");
        }

    }

}
