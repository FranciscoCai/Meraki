using UnityEngine;

public class Wolf_Howl : StateMachineBehaviour
{
    private UnityEngine.AI.NavMeshAgent m_agent;
    public LayerMask movableObjectLayer;
    private Animator m_animator;
    private DataWolf m_dataWolf;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_agent = animator.GetComponent<UnityEngine.AI.NavMeshAgent>();
        m_agent.isStopped = true;
        RaycastHit[] hits = Physics.SphereCastAll(m_agent.transform.position, 4f, Vector3.up, 0.01f, movableObjectLayer); // 3f es scale y 0.01 es punto central
        if (hits != null && hits.Length > 0)
        {
            for (int i = 0; i < hits.Length; i++)
            {
                Destroy(hits[i].collider.gameObject);
            }
        }
        m_dataWolf = animator.GetComponent<DataWolf>();
        m_animator = m_dataWolf.D_animation;
        m_animator.SetTrigger("T_Grito");
    }
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_animator.speed = 0f;
    }
}
