using UnityEngine;

public class Painter_Idle : StateMachineBehaviour
{
    private UnityEngine.AI.NavMeshAgent agent;
    private Contenedor contenedorScript;
    private bool changeState;
    private bool isScared = false;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("Idle", false);
        agent = animator.GetComponent<UnityEngine.AI.NavMeshAgent>();
        contenedorScript = animator.GetComponent<Contenedor>();
        agent.isStopped = true;

    }
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (GameManager.Instance.distance < 10)
        {
            isScared = true;
        }
        else
        {
            isScared = false;
        }
        //changeState = GameManager.Instance.changeState;

        //if (changeState == true && animator.GetBool("StunContainer") == false)
        //{
        //    animator.SetTrigger("Move");
        //}
        //if (changeState == true && animator.GetBool("StunContainer"))
        //{
        //    animator.SetTrigger("Stun");
        //}
    }
}
