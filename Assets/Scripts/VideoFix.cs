using UnityEngine;
using UnityEngine.Video;

public class VideoFix : MonoBehaviour
{
    private void Start()
    {
        // Coger el componente VideoPlayer del mismo GameObject
        VideoPlayer vp = GetComponent<VideoPlayer>();

        if (vp != null)
        {
            vp.time = 0;  // Fuerza el inicio en el frame 0
            vp.Play();    // Reproduce desde ahí
        }
    }
}