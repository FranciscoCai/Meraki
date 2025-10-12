using UnityEngine;
using UnityEngine.AI;

public class Tortuga_Swim : StateMachineBehaviour
{
    GameObject Tortuga;
    private NavMeshAgent agent;
    private int destPoint = 0;
    public float rayDistance = 6f;
    public GameObject[] points;  

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Tortuga = animator.gameObject;
        points = AIDirector.Instance.GetTortugaPoint();

        agent = Tortuga.GetComponent<NavMeshAgent>();

        agent.autoBraking = false;

        GotoNextPoint();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
            GotoNextPoint();

        Vector3 forwardDirection = Tortuga.transform.right;

        if (Physics.Raycast(Tortuga.transform.position + Vector3.up * 1.5f, forwardDirection, out RaycastHit hitInfo, rayDistance))
        {
            if (hitInfo.collider.gameObject.CompareTag("Lucy"))
            {
                animator.SetTrigger("ToParalizado");
            }
        }
        Debug.DrawRay(Tortuga.transform.position + Vector3.up * 1.5f, forwardDirection * rayDistance, Color.red);
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

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

