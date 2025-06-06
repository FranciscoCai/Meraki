using UnityEngine;
using UnityEngine.AI;

public class PaintBottle : MonoBehaviour
{
    [SerializeField] GameObject p_Dino;
    [SerializeField] Transform instnatiatePoint;
    [SerializeField] float DinoVelocity;
    private void OnTriggerEnter(Collider other)
    {
       
        if (other.CompareTag("Pintor"))
        {
            GameObject findDino = GameObject.FindWithTag("Dino");
            if(findDino == null)
            {
                GameObject Dino = Instantiate(p_Dino,instnatiatePoint.position,instnatiatePoint.rotation);
                if(DinoVelocity != 0)
                {
                    Dino.GetComponent<NavMeshAgent>().speed = DinoVelocity;
                }
            }
            Destroy(gameObject);
        }
    }
}

