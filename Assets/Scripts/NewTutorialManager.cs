using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;

public class NewTutorialManager : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private RectTransform mensajePanel;
    [SerializeField] private TextMeshProUGUI tutorialText;
    [SerializeField] private GameObject wall;
    [SerializeField] private GameObject primerTurno;
    [SerializeField] private GameObject segundoTurno;

    [Header("Sprites de turno a animar")]
    [SerializeField] private List<RectTransform> turnSprites;

    private Dictionary<RectTransform, Vector3> escalasOriginales = new Dictionary<RectTransform, Vector3>();

    [Header("Mensajes iniciales (Teclado)")]
    [TextArea]
    [SerializeField]
    private List<string> mensajesIntroTeclado = new List<string>
    {
        "Meraki es un juego por turnos",
        "En la parte superior de la pantalla aparece quién tiene el turno",
        "Ahora mismo el turno es tuyo, por lo que puedes moverte aunque el resto del mundo permanezca estático",
        "Tu objetivo es conseguir que el Constructor llegue al final del nivel",
        "Para cederle el turno y que se mueva pulsa la tecla R"
    };

    [Header("Mensajes iniciales (Mando)")]
    [TextArea]
    [SerializeField]
    private List<string> mensajesIntroMando = new List<string>
    {
        "Meraki es un juego por turnos",
        "En la parte superior de la pantalla aparece quién tiene el turno",
        "Ahora mismo el turno es tuyo, por lo que puedes moverte aunque el resto del mundo permanezca estático",
        "Tu objetivo es conseguir que el Constructor llegue al final del nivel",
        "Para cederle el turno y que se mueva pulsa la cruzeta hacia arriba"
    };

    [Header("Mensaje de primer turno (Teclado)")]
    [TextArea]
    [SerializeField]
    private string mensajePrimerTurnoTeclado =
        "Puedes mover los objetos de juguete disparándolos con el click izquierdo (LMB)";

    [Header("Mensaje de primer turno (Mando)")]
    [TextArea]
    [SerializeField]
    private string mensajePrimerTurnoMando =
        "Puedes mover los objetos de juguete disparándolos con R2";

    [Header("Mensaje de segundo turno")]
    [TextArea]
    [SerializeField]
    private List<string> mensajesSegundoTurno = new List<string>
    {
        "Una vez se ha movido el objeto no se podrá volver a utilizar hasta pasados unos turnos",
        "Cuando un objeto esté en cooldown será de color rojo",
    };

    private bool esperandoInput = false;
    private bool primerMensaje = false;
    private Vector3 escalaObjetivo = Vector3.one;
    private int mensajeActual = 0;

    private Coroutine pulso0;
    private Coroutine pulso1;

    public NavMeshAgent agent;

    private void Awake()
    {
        agent.speed = 0f;
        foreach (var sprite in turnSprites)
        {
            if (sprite != null && !escalasOriginales.ContainsKey(sprite))
                escalasOriginales[sprite] = sprite.localScale;
        }
    }

    private bool UsandoMando()
    {
        var joysticks = Input.GetJoystickNames();
        if (joysticks == null || joysticks.Length == 0)
            return false;

        foreach (var j in joysticks)
            if (!string.IsNullOrEmpty(j))
                return true;

        return false;
    }

    public void LanzarPrimerMensaje()
    {
        agent.speed = 4f;

        mensajePanel.gameObject.SetActive(false);

        List<string> mensajes = UsandoMando() ? mensajesIntroMando : mensajesIntroTeclado;

        if (mensajes.Count > 0)
            MostrarMensaje(mensajes[mensajeActual]);
    }

    private void Update()
    {
        if (esperandoInput && Input.anyKeyDown)
            StartCoroutine(OcultarAnimado());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == primerTurno)
        {
            if (wall != null)
                wall.SetActive(false);
           
            if (primerTurno != null)
                primerTurno.SetActive(false);
      
            StartCoroutine(EsperarUnSegundoDos());
        }
        if (other.gameObject == segundoTurno)
        {
            if (segundoTurno != null)
                segundoTurno.SetActive(false);

            List<string> mensajesDos = mensajesSegundoTurno;

            if (mensajeActual < mensajesDos.Count)
            {
                MostrarMensaje(mensajesDos[mensajeActual]);
                mensajeActual++; 
            }

        }
    }

    private void MostrarMensaje(string mensaje)
    {
        tutorialText.text = mensaje;
        mensajePanel.gameObject.SetActive(true);

        mensajePanel.localScale = Vector3.zero;
        Time.timeScale = 0f;

        
        if (mensajeActual == 1)
        {
            IniciarPulso(turnSprites[0], ref pulso0);
            IniciarPulso(turnSprites[1], ref pulso1);
        }
        else if (mensajeActual == 2)
        {
            DetenerPulso(turnSprites[1], ref pulso1);
        }
        else if (mensajeActual == 3)
        {
            DetenerPulso(turnSprites[0], ref pulso0);
            IniciarPulso(turnSprites[1], ref pulso1);
        }
        else if (mensajeActual == 4)
        {
            DetenerPulso(turnSprites[1], ref pulso1);
            primerMensaje = true;
        }

        StartCoroutine(Escalar(mensajePanel, escalaObjetivo, 0.3f, () =>
        {
            StartCoroutine(EsperarUnSegundo());
        }));
    }

    private IEnumerator OcultarAnimado()
    {
        esperandoInput = false;

        yield return Escalar(mensajePanel, Vector3.zero, 0.3f, () =>
        {
            mensajePanel.gameObject.SetActive(false);

            List<string> mensajes = UsandoMando() ? mensajesIntroMando : mensajesIntroTeclado;

            if (mensajeActual < mensajes.Count - 1 && !primerMensaje)
            {
                mensajeActual++;
                MostrarMensaje(mensajes[mensajeActual]);
            }
            else
            {
                Time.timeScale = 1f;
                DetenerPulso(turnSprites[0], ref pulso0);
                DetenerPulso(turnSprites[1], ref pulso1);
            }
        });
    }

    private IEnumerator EsperarUnSegundo()
    {
        esperandoInput = false;
        yield return new WaitForSecondsRealtime(1f);
        esperandoInput = true;
        
    }

    private IEnumerator EsperarUnSegundoDos()
    {
        yield return new WaitForSecondsRealtime(0.5f);

        primerMensaje = true;
        mensajeActual = 0;

        if (UsandoMando())
            MostrarMensaje(mensajePrimerTurnoMando);
        else
            MostrarMensaje(mensajePrimerTurnoTeclado);
    }

    private IEnumerator Escalar(RectTransform target, Vector3 objetivo, float duracion, System.Action onComplete)
    {
        Vector3 inicio = target.localScale;
        float tiempo = 0f;

        while (tiempo < duracion)
        {
            tiempo += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(tiempo / duracion);
            target.localScale = Vector3.Lerp(inicio, objetivo, t);
            yield return null;
        }

        target.localScale = objetivo;
        onComplete?.Invoke();
    }

   
    private void IniciarPulso(RectTransform target, ref Coroutine rutina)
    {
        if (target == null) return;
        if (rutina != null) StopCoroutine(rutina);
        rutina = StartCoroutine(Pulso(target));
    }

    private void DetenerPulso(RectTransform target, ref Coroutine rutina)
    {
        if (rutina != null)
        {
            StopCoroutine(rutina);
            rutina = null;
        }
        if (target != null && escalasOriginales.ContainsKey(target))
            target.localScale = escalasOriginales[target];
    }

    private IEnumerator Pulso(RectTransform target)
    {
        float duracion = 0.6f;
        float escalaMax = 1.3f;
        Vector3 original = escalasOriginales[target];

        while (true)
        {
            float t = 0;
            while (t < duracion)
            {
                t += Time.unscaledDeltaTime;
                target.localScale = Vector3.Lerp(original, original * escalaMax, t / duracion);
                yield return null;
            }
            t = 0;
            while (t < duracion)
            {
                t += Time.unscaledDeltaTime;
                target.localScale = Vector3.Lerp(original * escalaMax, original, t / duracion);
                yield return null;
            }
        }
    }
}
