using UnityEngine;
using UnityEngine.UI;
using System.Timers;
using System.Collections;
using UnityEngine.AI;

public class Wolf_Follow : StateMachineBehaviour
{
    private UnityEngine.AI.NavMeshAgent m_agent;
    private DataWolf m_dataWolf;
    private Animator m_animator;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_agent = animator.GetComponent<UnityEngine.AI.NavMeshAgent>();
        m_dataWolf = animator.GetComponent<DataWolf>();
        m_animator = m_dataWolf.D_animation;
        m_animator.SetTrigger("T_Andar");
        m_agent.isStopped = false;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_agent.destination = m_dataWolf.D_objetivo.position;

    }
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (ClosePointPosible())
        {
            GameManager.Instance.SetDinoEfect(DinoEfect.Howl);
        }
        m_animator.speed = 0f;
        //else if (m_agent.pathStatus == NavMeshPathStatus.PathPartial || m_agent.pathStatus == NavMeshPathStatus.PathInvalid)
        //{
        //    Debug.Log("El agente no puede alcanzar el destino por completo.");
        //}
    }
    bool ClosePointPosible()
    {
        if (!m_agent.pathPending && m_agent.remainingDistance <= 0.2f)
        {
            if (!m_agent.hasPath || m_agent.velocity.sqrMagnitude < 0.01f)
            {
                return true;
            }
        }
        return false;
    }
}