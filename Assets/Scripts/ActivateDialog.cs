using UnityEngine;

public class ActivateDialog : MonoBehaviour
{
    public bool activateDialogByCode = false;
    public GameObject Goal;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject == Goal)
        {
            activateDialogByCode = true;
        }
    }
}
