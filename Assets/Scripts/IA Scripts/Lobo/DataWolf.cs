using UnityEngine;
using UnityEngine.AI;

public class DataWolf : MonoBehaviour
{
    public bool D_hadBeenStuned = false;
    [SerializeField] private GameObject D_modelo;
    public Animator D_animation;

    public Transform D_objetivo;
    public Renderer D_objectRenderer;
    public Color D_originalColor;
    [SerializeField]private Color D_StunColor = Color.blue;
    [SerializeField] private Color D_AngryColor = Color.red;
    private Animator D_animator;
    private NavMeshAgent D_agent;
    [SerializeField] private LineRenderer D_lineRenderer;

    private void Start()
    {
        GameObject pintor = GameObject.FindWithTag("Pintor");
        if (pintor != null)
        {
            D_objetivo = pintor.transform;
            D_objetivo.GetComponent<Win_Lose>().Wolf = gameObject;
        }
        D_animator = GetComponent<Animator>();
        D_objectRenderer = D_modelo.GetComponent<Renderer>();
        D_originalColor = D_objectRenderer.material.color;

        D_agent = GetComponent<NavMeshAgent>();

        D_agent.SetDestination(D_objetivo.transform.position);

        DrawPath();


        D_agent.isStopped = true;
    }
    private void Update()
    {
        DrawPath();
    }
    private void DrawPath()
    {
        if (D_agent.hasPath)
        {
            Vector3[] corners = D_agent.path.corners;

            for (int i = 0; i < corners.Length; i++)
            {
                corners[i] = corners[i] + Vector3.up * 0.2f; // Ajustar cada esquina hacia arriba
            }

            D_lineRenderer.positionCount = corners.Length;
            D_lineRenderer.SetPositions(corners);
        }
        else
        {
            D_lineRenderer.positionCount = 0;
        }
    }


    private void OnEnable()
    {
        GameManager.OnStartTurn += DinoStartEfect;
        GameManager.OnPassTurn += DinoMoveEfect;
        GameManager.OnStopTurn += DinoStopEfect;
    }
    private void OnDisable()
    {
        GameManager.OnStartTurn -= DinoStartEfect;
        GameManager.OnPassTurn -= DinoMoveEfect;
        GameManager.OnStopTurn -= DinoStopEfect;
    }
    public void ChangeDinoStunColor()
    {
        D_objectRenderer.material.color = D_StunColor;
    }
    public void ChangeDinoAngryColor()
    {
        D_objectRenderer.material.color = D_AngryColor;
    }
    public void ChangeDinoOriginalColor()
    {
        D_objectRenderer.material.color = D_originalColor;
    }
    private void DinoStartEfect()
    {
        DinoEfect d_dinoEfect = GameManager.Instance.GetDinoEfect();
        if (d_dinoEfect == DinoEfect.Howl)
        {
            GameManager.Instance.SetPintorEfect(PintorEfect.Stun);
            GameManager.Instance.SetConstructorEfect(ConstructorEfect.Stun);
        }
    }

        private void DinoMoveEfect()
    {
        DinoEfect d_dinoEfect = GameManager.Instance.GetDinoEfect();
        if (d_dinoEfect == DinoEfect.Follow)
        {
            ChangeDinoOriginalColor();
            D_animator.SetTrigger("Follow");
        }
        else if (d_dinoEfect == DinoEfect.Stun)
        {
            GameManager.Instance.SetDinoEfect(DinoEfect.Howl);
            ChangeDinoStunColor();
            D_animator.SetTrigger("Stun");
        }
        else if (d_dinoEfect == DinoEfect.Howl)
        {
            GameManager.Instance.SetDinoEfect(DinoEfect.Angry);
            ChangeDinoOriginalColor();
            D_animator.SetTrigger("Howl");
        }
        else if (d_dinoEfect == DinoEfect.Angry)
        {
            ChangeDinoAngryColor();
            GameManager.Instance.SetDinoEfect(DinoEfect.Follow);
            D_animator.SetTrigger("Angry");
        }
    }
    private void DinoStopEfect()
    {
        D_animator.SetTrigger("Idle");
    }
}
