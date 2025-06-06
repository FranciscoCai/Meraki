using UnityEngine;


public class Contenedor : MonoBehaviour
{
    public Transform objetivo;
    public Renderer objectRenderer;
    public Color originalColor;
    private Color newColor = Color.blue;

    private void Start()
    {
        objectRenderer = gameObject.GetComponent<Renderer>();
        originalColor = objectRenderer.material.color;
    }
    public void ChangePintorStunColor()
    {
        objectRenderer.material.color = newColor;
    }
    public void ChangePintorOriginalColor()
    {
        objectRenderer.material.color = originalColor;
    }

}
