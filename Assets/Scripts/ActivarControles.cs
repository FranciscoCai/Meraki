using UnityEngine;

public class ActivarControles : MonoBehaviour
{
    public Transform player;           // Referencia al jugador
    public GameObject targetToActivate; // GameObject que se activará
    public float triggerDistance = 5f; // Distancia para activar
    public float timeToTrigger = 3f;   // Tiempo que debe permanecer en rango

    private float timer = 0f;
    private bool hasActivated = false;

    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= triggerDistance)
        {
            timer += Time.deltaTime;

            if (timer >= timeToTrigger && !hasActivated)
            {
                targetToActivate.SetActive(true);
                hasActivated = true;
            }
        }
        else
        {
            timer = 0f; // Reinicia el tiempo si se sale del rango
        }
    }
}
