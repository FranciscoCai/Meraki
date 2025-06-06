using UnityEngine;

public class DialogCameraManager : MonoBehaviour
{
    public Animator cameraAnimator;

    public void SetCameraTrigger(string triggerName)
    {
        if (cameraAnimator != null)
        {
            cameraAnimator.SetTrigger(triggerName);
        }
        else
        {
            Debug.LogWarning("Animator de c�mara no asignado en " + gameObject.name);
        }
    }
}
