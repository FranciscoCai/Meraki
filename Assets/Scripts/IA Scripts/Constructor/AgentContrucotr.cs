using System;
using UnityEngine;
using UnityEngine.AI;

public class AgentContrucotr : MonoBehaviour
{

    public Transform[] waypoints;
    public int currentWaypointIndex;
    private Animator C_animator;
    public ConstructorEfect c_ActualConstructorEfect;


    public Renderer C_objectRenderer;
    public Material C_freezeMaterial;
    public Material C_initialMaterial;

    private int C_stunTimer = 0;

    private void Start()
    {
        C_animator = GetComponent<Animator>();
    }
    private void OnEnable()
    {
        GameManager.OnPassTurn += ConstructorMoveEfect;
        GameManager.OnStopTurn += ConstructorStopEfect;
    }


    private void OnDisable()
    {
        GameManager.OnPassTurn -= ConstructorMoveEfect;
        GameManager.OnStopTurn -= ConstructorStopEfect;
    }
    private void ConstructorMoveEfect()
    {
        ConstructorEfect c_constructorEfect = GameManager.Instance.GetConstructorEfect();
        Debug.Log(c_constructorEfect);
        if (c_constructorEfect == ConstructorEfect.TurtleWalk)
        {
            C_objectRenderer.material = C_initialMaterial;
            C_animator.SetTrigger("ToTurtleWalk");
        }
        else if (c_constructorEfect == ConstructorEfect.Move)
        {
            C_objectRenderer.material = C_initialMaterial;
            C_animator.SetTrigger("ToMove");
        }
        else if (c_constructorEfect == ConstructorEfect.Stun)
        {
            C_stunTimer++;
            if (C_stunTimer == 1)
            {
                C_animator.SetTrigger("ToStun");
                C_objectRenderer.material = C_freezeMaterial;
            }
            if (C_stunTimer == 2)
            {
                C_animator.SetTrigger("ToStun");
                GameManager.Instance.SetConstructorEfect(c_ActualConstructorEfect);
                C_stunTimer = 0;
            }
        }
    }
    private void ConstructorStopEfect()
    {
        ConstructorEfect c_constructorEfect = GameManager.Instance.GetConstructorEfect();
        C_animator.SetTrigger("ToIdle");
        if (GameManager.Instance.GetConstructorEfect() != ConstructorEfect.Stun)
        {
            if (c_ActualConstructorEfect == ConstructorEfect.TurtleWalk)
            {
                GameManager.Instance.SetConstructorEfect(ConstructorEfect.TurtleWalk);
            }
            else if (c_ActualConstructorEfect == ConstructorEfect.Move)
            {
                GameManager.Instance.SetConstructorEfect(ConstructorEfect.Move);
            }
        }
    }




    public void ChangeDinoStunColor()
    {
        C_objectRenderer.material = C_freezeMaterial;
    }
    public void ChangeDinoOriginalColor()
    {
        C_objectRenderer.material = C_initialMaterial;
    }
}
