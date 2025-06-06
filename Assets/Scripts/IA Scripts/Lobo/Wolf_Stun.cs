using UnityEngine;

public class Wolf_Stun : StateMachineBehaviour
{
    private UnityEngine.AI.NavMeshAgent m_agent;
    private Animator m_animator;
    private DataWolf m_dataWolf;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_agent = animator.GetComponent<UnityEngine.AI.NavMeshAgent>();
        m_dataWolf = animator.GetComponent<DataWolf>();
        m_animator = m_dataWolf.D_animation;
        m_animator.SetTrigger("T_Stun");
        m_agent.isStopped = true;
    }
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_animator.speed = 0f;
    }
}
