using UnityEngine;

public class Button : MonoBehaviour
{
    [SerializeField] private GameObject[] wallMoveable;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MovebleObject"))
        {
            for(int i = 0; i < wallMoveable.Length; i++)
            {
                wallMoveable efectWall = wallMoveable[i].GetComponent<wallMoveable>();
                efectWall.wallEnterEfect();
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("MovebleObject"))
        {
            for (int i = 0; i < wallMoveable.Length; i++)
            {
                wallMoveable efectWall = wallMoveable[i].GetComponent<wallMoveable>();
                efectWall.wallExitEfect();
            }
        }
    }
}
