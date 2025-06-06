using UnityEngine;
using System.Collections.Generic;

public class CameraChangeRound : MonoBehaviour
{

    public List<Transform> targets;      // Lista de objetivos que la cámara debe seguir
    public float minDistance = 10f;      // Distancia mínima a la que la cámara puede acercarse
    public float maxDistance = 50f;      // Distancia máxima que la cámara puede alejarse
    public float paddingDistance = 2f;   // Factor de padding para el campo de visión
    public float smoothTime = 0.5f;      // Tiempo de suavizado para el movimiento de la cámara

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

        // Encuentra el punto central y la distancia máxima de los objetivos
        Vector3 centerPoint = GetCenterPoint();
        float greatestDistance = GetGreatestDistance(centerPoint);

        // Calcula la distancia deseada de la cámara basada en la distancia máxima y el padding
        float desiredDistance = Mathf.Lerp(minDistance, maxDistance, greatestDistance / maxDistance) + paddingDistance;

        // Ajusta la posición de la cámara suavemente hacia el nuevo punto de destino
        Vector3 desiredPosition = centerPoint - transform.forward * desiredDistance;
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothTime);

        // Ajusta el campo de visión (FOV) de la cámara para incluir a todos los objetivos
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

    // Calcula la distancia entre el punto central y los objetos más lejanos
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