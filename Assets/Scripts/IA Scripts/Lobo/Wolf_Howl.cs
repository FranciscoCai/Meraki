using UnityEngine;

public class Wolf_Howl : StateMachineBehaviour
{
    private UnityEngine.AI.NavMeshAgent m_agent;
    public LayerMask movableObjectLayer;

    [Header("Audio Settings")]
    public AudioClip howlSound;

    private AudioSource audioSource;
    private Animator m_animator;
    private DataWolf m_dataWolf;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_agent = animator.GetComponent<UnityEngine.AI.NavMeshAgent>();
        m_agent.isStopped = true;

        // 🔊 Busca el AudioSource automáticamente
        audioSource = animator.GetComponent<AudioSource>();
        if (audioSource == null)
        {
            // Si no tiene, crea uno temporal
            audioSource = animator.gameObject.AddComponent<AudioSource>();
        }

        // Elimina objetos cercanos
        RaycastHit[] hits = Physics.SphereCastAll(m_agent.transform.position, 4f, Vector3.up, 0.01f, movableObjectLayer);
        if (hits != null && hits.Length > 0)
        {
            foreach (var hit in hits)
            {
                Destroy(hit.collider.gameObject);
            }
        }

        m_dataWolf = animator.GetComponent<DataWolf>();
        m_animator = m_dataWolf.D_animation;
        m_animator.SetTrigger("T_Grito");

        // Reproduce el sonido
        if (howlSound != null)
            audioSource.PlayOneShot(howlSound);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_animator.speed = 0f;
    }
}

