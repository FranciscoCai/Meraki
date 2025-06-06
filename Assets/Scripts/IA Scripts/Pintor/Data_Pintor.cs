using UnityEngine;

public class Data_Pintor : MonoBehaviour
{
    public Transform P_objetivo;
    [SerializeField] private GameObject P_modelo;

    private Renderer P_objectRenderer;
    [SerializeField] private Color P_originalColor;
    [SerializeField] private Color P_newColor = Color.blue;
    [SerializeField] private Animator P_animator;

    private int g_stunTimer = 0;
    private void Start()
    {
        P_objectRenderer = P_modelo.GetComponent<Renderer>();
        P_originalColor = P_objectRenderer.material.color;
        P_animator = gameObject.GetComponent<Animator>();
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
        GameManager.OnPassTurn += PintorMoveEfect;
        GameManager.OnStopTurn += PintorStopEfect;
    }
    private void OnDisable()
    {
        GameManager.OnPassTurn -= PintorMoveEfect;
        GameManager.OnStopTurn -= PintorStopEfect;
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
}
