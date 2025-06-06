using UnityEngine;

public class Controls : MonoBehaviour
{
    void Update()
    {
        // Apunta hacia la cámara
        Vector3 dir = Camera.main.transform.position - transform.position;
        dir.y = 0; // Opcional: mantenerlo solo en el plano horizontal

        // Crear la rotación hacia la cámara
        Quaternion lookRot = Quaternion.LookRotation(dir);

        // Aplicar rotación y corregir para que use el eje "arriba" del plano como frontal
        transform.rotation = lookRot * Quaternion.Euler(90, 0, 0);  // Corrige que el plano mire con su cara
    }
}
