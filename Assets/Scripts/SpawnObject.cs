using UnityEngine;

public class SpawnObject : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPoint;

    private void OnEnable()
    {
        if (SpawnManager.Instance != null)
        {
            SpawnManager.Instance.OnSpawn += SpawnEfect;
        }
    }
    private void OnDisable()
    {
        if (SpawnManager.Instance != null)
        {
            SpawnManager.Instance.OnSpawn -= SpawnEfect;
        }
    }
    public void SpawnEfect(int _spawnCount)
    {
       gameObject.transform.position = spawnPoint[_spawnCount].position;
        Physics.SyncTransforms();
        gameObject.transform.rotation = spawnPoint[_spawnCount].rotation;
    }
}
