using System.Collections;
using UnityEngine;

public class Turtle_Move : StateMachineBehaviour
{
    private GameObject m_Turtle;
    private Transform[] m_Points;
    private float m_Speed;
    public int m_DestCount = 0;
    //[SerializeField] private float m_CdTime;
    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_Turtle = animator.gameObject;
        m_Points = m_Turtle.GetComponent<DataTurtle>().MovePoints;
        m_Speed = m_Turtle.GetComponent<DataTurtle>().speed;
        //m_Turtle.GetComponent<DataTurtle>().StartCoroutine(CDMove());
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_Turtle.transform.position = Vector3.MoveTowards(m_Turtle.transform.position, m_Points[m_DestCount].position, m_Speed * Time.deltaTime);
        if (Vector3.Distance(m_Turtle.transform.position, m_Points[m_DestCount].position) < 0.25f)
        {
            MoveTo();
        }
    }
    private void MoveTo()
    {
        if (m_Points.Length == 0)
            return;
        m_DestCount = (m_DestCount + 1) % m_Points.Length;
    }
    //private IEnumerator CDMove()
    //{
    //    yield return new WaitForSeconds(m_CdTime);
    //    m_CanShoot = true;
    //}
    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
