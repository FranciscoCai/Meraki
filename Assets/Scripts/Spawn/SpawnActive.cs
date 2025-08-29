using UnityEngine;

public class SpawnActive : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (SpawnManager.Instance != null)
            {
                SpawnManager.Instance.ActiveSpawn();
            }
        }
    }
}
