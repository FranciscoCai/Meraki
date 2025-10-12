using Unity.VisualScripting;
using UnityEngine;

public class AIDirector : MonoBehaviour
{
    public static AIDirector Instance;

    public GameObject[] Tortuga;
    public GameObject[] TortugaPoint;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        Tortuga = GameObject.FindGameObjectsWithTag("Tortuga");
        TortugaPoint = GameObject.FindGameObjectsWithTag("TortugaPoint");
    }

    void Update()
    {
        
    }

    public GameObject[] GetTortugaPoint()
    {
        return TortugaPoint;
    }
}
