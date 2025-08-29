using UnityEngine;

public class SpawnCountChange : MonoBehaviour
{
    [SerializeField] private int _spawnCountChange = 1;
    [SerializeField] private string collideGameObjectTag;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(collideGameObjectTag))
        {
            if (SpawnManager.Instance != null)
            {
                SpawnManager.Instance.ChangeSpawnCount(_spawnCountChange);
            }
        }
    }
}
