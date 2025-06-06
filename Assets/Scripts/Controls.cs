using UnityEngine;

public class Controls : MonoBehaviour
{
    void Update()
    {
        // Apunta hacia la c�mara
        Vector3 dir = Camera.main.transform.position - transform.position;
        dir.y = 0; // Opcional: mantenerlo solo en el plano horizontal

        // Crear la rotaci�n hacia la c�mara
        Quaternion lookRot = Quaternion.LookRotation(dir);

        // Aplicar rotaci�n y corregir para que use el eje "arriba" del plano como frontal
        transform.rotation = lookRot * Quaternion.Euler(90, 0, 0);  // Corrige que el plano mire con su cara
    }
}
