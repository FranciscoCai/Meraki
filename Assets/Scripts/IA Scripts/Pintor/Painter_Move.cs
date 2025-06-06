using UnityEngine;
using UnityEngine.UI;
using System.Timers;

public class Painter_Move : StateMachineBehaviour
{
    private UnityEngine.AI.NavMeshAgent agent;
    private Data_Pintor m_dataPintor;
    private bool isScared = false;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent = animator.GetComponent<UnityEngine.AI.NavMeshAgent>();
        m_dataPintor = animator.GetComponent<Data_Pintor>();

        agent.isStopped = false;
        agent.autoBraking = false;


    }
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.destination = m_dataPintor.P_objetivo.position;

        if (GameManager.Instance.distance < 10)
        {
            isScared = true;
        }
        else
        {
            isScared = false;
        }
        //timer += Time.deltaTime;

        //if (timer >= 2f)
        //{
        //    animator.SetTrigger("Idle");
        //}
    }
}
