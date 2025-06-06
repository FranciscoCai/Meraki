using UnityEngine;

public class ObjectCollisionCheck : MonoBehaviour
{
    public bool m_isCollision = false;
    public Renderer m_materialToHologram;
    public GameObject m_limitGround;
    public Material m_materialHologram;

    // Se llama cada frame en el que el objeto est¨¢ en colisi¨®n con otro
    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Button"))
        {
            return;
        }
        m_isCollision = true;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Button"))
        {
            return;
        }
        m_isCollision = false;
    }
    private void OnCollisionStay(Collision other)
    {if (other.gameObject.layer == LayerMask.NameToLayer("Button"))
        {
            return;
        }
        m_isCollision = true; }
    private void OnCollisionExit(Collision other)
    {if (other.gameObject.layer == LayerMask.NameToLayer("Button"))
        {
            return;
        }
        m_isCollision = false; }
}
