using UnityEngine;
using UnityEngine.UI;
using System.Timers;

public class Wolf_Idle : StateMachineBehaviour
{
    private UnityEngine.AI.NavMeshAgent agent;
    private DataWolf m_dataWolf;
    private Animator m_animator;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent = animator.GetComponent<UnityEngine.AI.NavMeshAgent>();
        m_dataWolf = animator.GetComponent<DataWolf>();
        m_animator = m_dataWolf.D_animation;
        agent.isStopped = true;
    }
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_animator.speed = 1f;
    }
}
