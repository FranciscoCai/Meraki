using UnityEngine;
using TMPro;

public class wolfText : MonoBehaviour
{
    public static wolfText Instance;
    [Header("Referencias UI")]
    public GameObject panel;
    public TextMeshProUGUI textoUI;

    [Header("Ajustes")]
    [Tooltip("Segundos (tiempo real) antes de poder cerrar.")]
    public float retrasoParaCerrar = 1f;

    private bool mostrando = false;
    private float tiempoDesbloqueo = 0f;         // En tiempo real
    private float timeScalePrevio = 1f;

    void Awake()
    {
        // Singleton seguro
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        if (panel != null) panel.SetActive(false);
    }

    void Update()
    {
        if (!mostrando || panel == null || !panel.activeSelf) return;

        // Ya se cumplió el tiempo real mínimo para poder cerrar
        bool puedeCerrar = Time.realtimeSinceStartup >= tiempoDesbloqueo;

        if (puedeCerrar)
        {
            // Teclado o cualquier botón del ratón
            if (Input.anyKeyDown || Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2))
            {
                OcultarMensaje();
            }
        }
    }

    public void MostrarMensaje(string mensaje)
    {
        if (panel != null) panel.SetActive(true);
        if (textoUI != null) textoUI.text = mensaje;

        // Pausar el juego guardando el valor previo (por si ya estaba escalado)
        timeScalePrevio = Time.timeScale;
        Time.timeScale = 0f;

        mostrando = true;
        // Marcar el instante real a partir del cual se puede cerrar
        tiempoDesbloqueo = Time.realtimeSinceStartup + Mathf.Max(0f, retrasoParaCerrar);
    }

    public void OcultarMensaje()
    {
        if (panel != null) panel.SetActive(false);

        // Restaurar el timeScale previo
        Time.timeScale = timeScalePrevio;

        mostrando = false;
    }
}
