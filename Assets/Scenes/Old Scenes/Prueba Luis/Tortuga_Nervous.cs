using UnityEngine;
using UnityEngine.AI;

public class Tortuga_Nervous : StateMachineBehaviour
{
    GameObject Tortuga;
    private NavMeshAgent agent;
    private int destPoint = 0;
    public float rayDistance = 6f;
    public GameObject[] points;
    float time;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent = animator.GetComponent<NavMeshAgent>();
        Tortuga = animator.gameObject;

        agent.isStopped = false;        
        points = AIDirector.Instance.GetTortugaPoint();

        agent.speed = 3;

        agent.autoBraking = false;

        GotoNextPoint();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
            GotoNextPoint();
        time += Time.deltaTime;
        if (time >= 6)
        {
            time = 0;

            animator.SetTrigger("ToSwim");
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.speed = 1;
    }

    override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
    void GotoNextPoint()
    {
        if (points.Length == 0)
            return;

        agent.destination = points[destPoint].transform.position;

        destPoint = (destPoint + 1) % points.Length;
    }
}
