using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ChangeScene : MonoBehaviour
{
    public GameObject Goal;
    public string SceneToChange;
    public Animator transition;

    [Header("Audio Settings")]
    public AudioSource audioSource;
    public AudioClip winSound;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject == Goal)
        {
            audioSource.PlayOneShot(winSound);
            StartCoroutine(LoadLevel());
            Debug.Log("Collide");
           
        }
        IEnumerator LoadLevel()
        {
            transition.SetTrigger("Start");
  

            yield return new WaitForSeconds(1);
            if(SpawnManager.Instance != null)
            {
                SpawnManager.Instance.ChangeSpawnCount(0);
            }

            SceneManager.LoadScene(SceneToChange);
        }
        
    }
}
