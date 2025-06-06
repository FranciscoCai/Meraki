using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ChangeScene : MonoBehaviour
{
    public GameObject Goal;
    public string SceneToChange;
    public Animator transition;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject == Goal)
        {
            StartCoroutine(LoadLevel());
            Debug.Log("Collide");
          
        }
        IEnumerator LoadLevel()
        {
            transition.SetTrigger("Start");

            yield return new WaitForSeconds(1);

            SceneManager.LoadScene(SceneToChange);
        }
        
    }
}
