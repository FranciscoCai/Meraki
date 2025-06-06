using UnityEngine;

public class Dino_Agent : MonoBehaviour
{
    public GameObject Constructor;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        Constructor = GameObject.FindWithTag("Constructor");
        Debug.Log("detecta al constructor");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
