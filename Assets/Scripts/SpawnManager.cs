using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public delegate void OnSpawnHandler(int _spawnCount);

    public event OnSpawnHandler OnSpawn;

    public static SpawnManager Instance;

    private int _spawnCount = 0;
    public int SpawnCount
    {
        get { return _spawnCount; }
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void ChangeSpawnCount(int count)
    {
        _spawnCount = count;
    }
    public void ActiveSpawn()
    {
        OnSpawn?.Invoke(_spawnCount);
    }
}
