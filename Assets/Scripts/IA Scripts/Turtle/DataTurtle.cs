using UnityEngine;

public class DataTurtle : MonoBehaviour
{
    public Transform[] MovePoints;
    public float speed;
    private Animator T_animator;
    public Renderer D_objectRenderer;
    public bool isStoped = false;
    public bool T_CanShoot = true;

    public TurtleEfect t_turtleEfect = TurtleEfect.Move;
    public Material t_freezeMaterial;
    public Material t_initialMaterial;
    public Animator t_turtleAnimator;


    private void OnEnable()
    {
        GameManager.OnPassTurn += TurtleMoveEfect;
        GameManager.OnStopTurn += TurtleStopEfect;
    }


    private void OnDisable()
    {
        GameManager.OnPassTurn -= TurtleMoveEfect;
        GameManager.OnStopTurn -= TurtleStopEfect;
    }
    private void TurtleMoveEfect()
    {
        if(t_turtleEfect == TurtleEfect.Move)
        {
            T_animator.SetTrigger("To_Move");
        }
        else if(t_turtleEfect == TurtleEfect.Down)
        {
            T_animator.SetTrigger("To_Down");
        }
        else if (t_turtleEfect == TurtleEfect.Stop)
        {
            T_animator.SetTrigger("To_Stop");
        }
        else if(t_turtleEfect == TurtleEfect.Up)
        {
            T_animator.SetTrigger("To_Up");
        }
        //if (c_constructorEfect == ConstructorEfect.TurtleWalk)
        //{
        //    C_animator.SetTrigger("ToTurtleWalk");
        //}
        //else if (c_constructorEfect == ConstructorEfect.Move)
        //{
        //    C_animator.SetTrigger("ToMove");
        //}
    }
    private void TurtleStopEfect()
    {
        T_animator.SetTrigger("To_Idle");
        if (t_turtleEfect == TurtleEfect.Move)
        {

        }
        else if (t_turtleEfect == TurtleEfect.Down)
        {
            t_turtleEfect = TurtleEfect.Stop;
        }
        else if (t_turtleEfect == TurtleEfect.Stop)
        {
            t_turtleEfect = TurtleEfect.Up;
        }
        else if (t_turtleEfect == TurtleEfect.Up)
        {
            t_turtleEfect = TurtleEfect.Move;
        }
    }


    void Start()
    {
        T_animator = GetComponent<Animator>();
        //D_objectRenderer.material.color = Color.blue;
    }

    // Update is called once per frame
    //public void TurtlehitEfect()
    //{
    //    AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
    //    if (stateInfo.IsName("Wait"))
    //    {
    //        animator.SetTrigger("ToMove");
    //    }
    //    else if (stateInfo.IsName("Move"))
    //    {
    //        if (animator.GetBehaviours<Turtle_Move>()[0].m_CanShoot)
    //        {
    //            animator.SetTrigger("ToStop");
    //        }
    //    }
    //}
}
