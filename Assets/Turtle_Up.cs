using System.Collections;
using UnityEngine;

public class Turtle_Up : StateMachineBehaviour
{
    private GameObject m_Turtle;
    [SerializeField] private float m_Speed;
    [SerializeField] private float m_MoveGrade;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_Turtle = animator.gameObject;
        m_Turtle.GetComponent<DataTurtle>().D_objectRenderer.material = m_Turtle.GetComponent<DataTurtle>().t_initialMaterial;
        m_Turtle.GetComponent<DataTurtle>().t_turtleAnimator.speed = 1f;
        animator.gameObject.GetComponent<DataTurtle>().StartCoroutine(CDMove(animator)); ;
    }
    private IEnumerator CDMove(Animator animator)
    {
        m_Turtle.GetComponent<DataTurtle>().isStoped = false;
        Vector3 endTransform = m_Turtle.transform.position;
        while (endTransform.y + m_MoveGrade > m_Turtle.transform.position.y)
        {
            animator.transform.position = Vector3.MoveTowards(
                   animator.transform.position,
                   new Vector3(animator.transform.position.x, endTransform.y + m_MoveGrade, animator.transform.position.z),
                   m_Speed * Time.deltaTime
               );
            yield return null;
        }

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
