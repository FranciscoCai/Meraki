using UnityEngine;

public class ActivarConstructor : MonoBehaviour
{
    [SerializeField] private GameObject activar;
    [SerializeField] private NewTutorialManager constructorTutorial;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == activar)
        {
            if (constructorTutorial != null)
            {
                constructorTutorial.LanzarPrimerMensaje();
                activar.SetActive(false);
            }
        }
    }
}