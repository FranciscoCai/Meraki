using UnityEngine;
using System.Collections;
public class Wolf_Angry : StateMachineBehaviour
{
    private UnityEngine.AI.NavMeshAgent m_agent;
    private DataWolf m_dataWolf;
    private float m_initialSpeed;
    private Animator m_animator;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_agent = animator.GetComponent<UnityEngine.AI.NavMeshAgent>();
        m_dataWolf = animator.GetComponent<DataWolf>();
        m_animator = m_dataWolf.D_animation;
        m_animator.SetTrigger("T_Andar");
        m_agent.isStopped = false;
        m_initialSpeed = m_agent.speed;
        m_agent.speed *= 1.5f;

    }
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_agent.destination = m_dataWolf.D_objetivo.position;
    }
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_agent.speed = m_initialSpeed;
        m_animator.speed = 0f;
    }
}
