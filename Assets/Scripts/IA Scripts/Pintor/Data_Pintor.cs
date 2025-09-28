using UnityEngine;
using UnityEngine.AI;

public class Data_Pintor : MonoBehaviour
{
    public Transform P_objetivo;
    [SerializeField] private GameObject P_modelo;

    private Renderer P_objectRenderer;
    [SerializeField] private Color P_originalColor;
    [SerializeField] private Color P_newColor = Color.blue;
    [SerializeField] private Animator P_animator;
    public Animator P_animatorAnimations;

    private NavMeshAgent agent;

    private int g_stunTimer = 0;
    private void Start()
    {
        P_objectRenderer = P_modelo.GetComponent<Renderer>();
        P_originalColor = P_objectRenderer.material.color;
        P_animator = gameObject.GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }
    public void ChangePintorStunColor()
    {
        P_objectRenderer.material.color = P_newColor;
    }
    public void ChangePintorOriginalColor()
    {
        P_objectRenderer.material.color = P_originalColor;
    }
    private void OnEnable()
    {
        if (SpawnManager.Instance != null)
        {
            SpawnManager.Instance.OnSpawn += PintorSpawnEfect;
        }
        GameManager.OnPassTurn += PintorMoveEfect;
        GameManager.OnStopTurn += PintorStopEfect;
    }
    private void OnDisable()
    {
        if (SpawnManager.Instance != null)
        {
            SpawnManager.Instance.OnSpawn -= PintorSpawnEfect;
        }
        GameManager.OnPassTurn -= PintorMoveEfect;
        GameManager.OnStopTurn -= PintorStopEfect;
    }
    public void PintorSpawnEfect(int _spawnCount)
    {
        GameManager.Instance.SetPintorEfect(PintorEfect.Move);
    }
    private void PintorMoveEfect()
    {
        PintorEfect p_pintorEfect = GameManager.Instance.GetPintorEfect();
        if (p_pintorEfect == PintorEfect.Stun)
        {
            g_stunTimer++;
            if (g_stunTimer == 1)
            {
                ChangePintorStunColor();
                P_animator.SetTrigger("Stun");
            }
            if (g_stunTimer == 2)
            {
                GameManager.Instance.SetPintorEfect(PintorEfect.Move);
                g_stunTimer = 0;
            }
        }
        else if (p_pintorEfect == PintorEfect.Move)
        {
            P_animator.SetTrigger("Move");
        }
    }
    private void PintorStopEfect()
    {
        PintorEfect p_pintorEfect = GameManager.Instance.GetPintorEfect();
        P_animator.SetTrigger("Idle");
        if (p_pintorEfect == PintorEfect.Move)
        {
            ChangePintorOriginalColor();
        }
    }

    void Update()
    {
        // Calcula la velocidad horizontal (sin contar la vertical)
        float speed = new Vector3(agent.velocity.x, 0, agent.velocity.z).magnitude;

        // Asigna la velocidad al par¨¢metro del Animator
        P_animatorAnimations.SetFloat("Speed", speed);
        Debug.Log(speed);
    }
}
