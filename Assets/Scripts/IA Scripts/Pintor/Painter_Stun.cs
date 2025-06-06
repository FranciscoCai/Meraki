using UnityEngine;

public class Painter_Stun : StateMachineBehaviour
{
    private UnityEngine.AI.NavMeshAgent agent;
    //private float timer = 0f;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Color

        agent = animator.GetComponent<UnityEngine.AI.NavMeshAgent>();

        agent.isStopped = true;

        //timer = 0f;
    }
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //timer += Time.deltaTime;
        //if (timer >= 2f && turno != 0)
        //{
        //    animator.SetTrigger("Idle");
        //}
        //if (timer >= 2f && turno == 0)
        //{
        //    animator.SetTrigger("Idle");
        //    objectRenderer.material.color = originalColor;
        //}        
        //if (turno == 2)
        //{
        //    animator.SetBool("StunContainer", false);
        //    turno = 0;
            

        //}

    }
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("Idle", false);
    }
}
