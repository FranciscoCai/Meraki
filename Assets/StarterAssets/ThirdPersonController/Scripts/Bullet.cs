using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Update is called once per frame
    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}
