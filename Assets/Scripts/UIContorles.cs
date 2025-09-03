using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIContorles : MonoBehaviour
{
    [Header("Referencias de UI")]
    public GameObject tecladoImagen;
    public GameObject mandoImagen;

    [Header("Cámara a comprobar (debe estar DESactivada para mostrar)")]
    public Camera camaraReferencia;

    [Header("Animación")]
    [Range(0.05f, 1f)] public float duracion = 1f;
    public AnimationCurve curva = AnimationCurve.EaseInOut(0, 0, 1, 1); 

    private enum Modo { Ninguno, Teclado, Mando }
    private Modo ultimoModoDetectado = Modo.Ninguno; 
    private Modo modoMostrado = Modo.Ninguno;      

    private Vector3 escalaTeclado;
    private Vector3 escalaMando;
    private Coroutine animacionActual;

    void Awake()
    {
        if (tecladoImagen) escalaTeclado = tecladoImagen.transform.localScale;
        if (mandoImagen) escalaMando = mandoImagen.transform.localScale;

    
        if (tecladoImagen) tecladoImagen.SetActive(false);
        if (mandoImagen) mandoImagen.SetActive(false);
    }

    void Update()
    {
        bool camaraDesactivada = camaraReferencia != null && !camaraReferencia.gameObject.activeInHierarchy;

        if (!camaraDesactivada)
        {
            OcultarTodo();
            return;
        }

        DetectarEntrada();

      
        if (ultimoModoDetectado != modoMostrado)
        {
            if (ultimoModoDetectado == Modo.Teclado)
                Mostrar(tecladoImagen, escalaTeclado, Modo.Teclado);
            else if (ultimoModoDetectado == Modo.Mando)
                Mostrar(mandoImagen, escalaMando, Modo.Mando);
        }
    }

    void DetectarEntrada()
    {
       
        var joysticks = Input.GetJoystickNames();
        bool hayMandoConectado = joysticks != null && joysticks.Length > 0 && !string.IsNullOrEmpty(joysticks[0]);

        if (hayMandoConectado)
        {
            ultimoModoDetectado = Modo.Mando;
        }
        else
        {
         
            if (Input.anyKeyDown || Mathf.Abs(Input.GetAxis("Mouse X")) > 0.1f || Mathf.Abs(Input.GetAxis("Mouse Y")) > 0.1f)
            {
                ultimoModoDetectado = Modo.Teclado;
            }
        }
    }

    void Mostrar(GameObject objetivo, Vector3 escalaFinal, Modo nuevoModo)
    {
      
        if (objetivo == tecladoImagen && mandoImagen) mandoImagen.SetActive(false);
        if (objetivo == mandoImagen && tecladoImagen) tecladoImagen.SetActive(false);

       
        if (animacionActual != null) StopCoroutine(animacionActual);

       
        objetivo.SetActive(true);
        objetivo.transform.localScale = Vector3.zero;
        animacionActual = StartCoroutine(AnimarEscala(objetivo.transform, escalaFinal));

        modoMostrado = nuevoModo;
    }

    IEnumerator AnimarEscala(Transform t, Vector3 destino)
    {
        float t0 = 0f;
        while (t0 < duracion)
        {
            t0 += Time.deltaTime;
            float p = Mathf.Clamp01(t0 / duracion);
            float eased = curva.Evaluate(p);
            t.localScale = Vector3.LerpUnclamped(Vector3.zero, destino, eased);
            yield return null;
        }
        t.localScale = destino;
    }

    void OcultarTodo()
    {
        if (animacionActual != null) StopCoroutine(animacionActual);
        if (tecladoImagen) tecladoImagen.SetActive(false);
        if (mandoImagen) mandoImagen.SetActive(false);
        modoMostrado = Modo.Ninguno;
    }
}