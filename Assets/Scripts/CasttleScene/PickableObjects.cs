using UnityEngine;

public class PickableObjects : MonoBehaviour
{
    public float rotationSpeed = 50f;        // Velocidad de rotación en grados por segundo
    public float floatAmplitude = 0.25f;     // Qué tan alto/flota el objeto
    public float floatFrequency = 1f;        // Frecuencia del movimiento vertical

    private Vector3 startPosition;

    void Start()
    {
        // Guardamos la posición inicial para usarla como base del movimiento vertical
        startPosition = transform.position;
    }

    void Update()
    {
        // Rotación sobre el eje Y
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);

        // Movimiento vertical de tipo senoidal
        float newY = startPosition.y + Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}
