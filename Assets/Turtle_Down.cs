using System.Collections;
using UnityEngine;

public class Turtle_Down : StateMachineBehaviour
{
    private GameObject m_Turtle;
    [SerializeField] private float m_Speed;
    [SerializeField] private float m_MoveGrade;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_Turtle = animator.gameObject;
        m_Turtle.GetComponent<DataTurtle>().D_objectRenderer.material = m_Turtle.GetComponent<DataTurtle>().t_freezeMaterial;
        m_Turtle.GetComponent<DataTurtle>().t_turtleAnimator.speed = 0f;
        animator.gameObject.GetComponent<DataTurtle>().StartCoroutine(CDMove(animator)); 

    }
    private IEnumerator CDMove(Animator animator)
    {
        Vector3 startlTransform = m_Turtle.transform.position;
        while (startlTransform.y - m_MoveGrade+0.1f < m_Turtle.transform.position.y)
        {
            animator.transform.position = Vector3.MoveTowards(
                   animator.transform.position,
                   new Vector3(animator.transform.position.x, startlTransform.y - m_MoveGrade, animator.transform.position.z),
                   m_Speed * Time.deltaTime
               );
            yield return null;
        }
        m_Turtle.GetComponent<DataTurtle>().isStoped = true;
    }

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
