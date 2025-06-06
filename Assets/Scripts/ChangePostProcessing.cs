using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ChangePostProcessing : MonoBehaviour
{
    [SerializeField] private float m_SaturationValue;

    public Volume globalVolume; 
    private void Experiment()
    {
        if (globalVolume.profile.TryGet(out ColorAdjustments colorAdjustments))
        {
            colorAdjustments.saturation.value = m_SaturationValue;
            Destroy(gameObject);
        }
        else
        {
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        Experiment();
    }
    private void Start()
    {
        globalVolume = GameObject.Find("GlobalVolume").GetComponent<Volume>();
    }
}
