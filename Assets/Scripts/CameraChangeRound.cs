using UnityEngine;
using System.Collections.Generic;

public class CameraChangeRound : MonoBehaviour
{

    public List<Transform> targets;      // Lista de objetivos que la c�mara debe seguir
    public float minDistance = 10f;      // Distancia m�nima a la que la c�mara puede acercarse
    public float maxDistance = 50f;      // Distancia m�xima que la c�mara puede alejarse
    public float paddingDistance = 2f;   // Factor de padding para el campo de visi�n
    public float smoothTime = 0.5f;      // Tiempo de suavizado para el movimiento de la c�mara

    private Vector3 velocity;            // Velocidad para el suavizado
    private Camera cam;

    private void Start()
    {
        cam = GetComponent<Camera>();
    }

    private void LateUpdate()
    {
        // Verifica si hay objetivos en la lista
        if (targets.Count == 0)
            return;

        // Encuentra el punto central y la distancia m�xima de los objetivos
        Vector3 centerPoint = GetCenterPoint();
        float greatestDistance = GetGreatestDistance(centerPoint);

        // Calcula la distancia deseada de la c�mara basada en la distancia m�xima y el padding
        float desiredDistance = Mathf.Lerp(minDistance, maxDistance, greatestDistance / maxDistance) + paddingDistance;

        // Ajusta la posici�n de la c�mara suavemente hacia el nuevo punto de destino
        Vector3 desiredPosition = centerPoint - transform.forward * desiredDistance;
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothTime);

        // Ajusta el campo de visi�n (FOV) de la c�mara para incluir a todos los objetivos
        cam.fieldOfView = Mathf.Lerp(40f, 60f, greatestDistance / maxDistance);  // Ajusta el FOV basado en la distancia
    }

    // Encuentra el punto central entre todos los objetivos
    Vector3 GetCenterPoint()
    {
        if (targets.Count == 1)
            return targets[0].position;

        Bounds bounds = new Bounds(targets[0].position, Vector3.zero);
        foreach (Transform target in targets)
        {
            bounds.Encapsulate(target.position);
        }
        return bounds.center;
    }

    // Calcula la distancia entre el punto central y los objetos m�s lejanos
    float GetGreatestDistance(Vector3 centerPoint)
    {
        float maxDistance = 0f;
        foreach (Transform target in targets)
        {
            float distance = Vector3.Distance(target.position, centerPoint);
            if (distance > maxDistance)
            {
                maxDistance = distance;
            }
        }
        return maxDistance;
    }
}