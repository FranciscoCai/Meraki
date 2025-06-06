using UnityEngine;
using UnityEngine.SceneManagement;

public class Win_Lose : MonoBehaviour
{
    public GameObject Wolf;
    public string SceneToChange;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject == Wolf)
        {
            Debug.Log("AAAAAAAAAAAA");
            SceneManager.LoadScene(SceneToChange);
        }
    }
    //private void OnTriggerStay(Collider collision)
    //{
    //    if (collision.gameObject == Goal)
    //    {
    //        SceneChangeContainer.Instance.SceneChangeToVictory();
    //    }
    //    if (collision.gameObject == Wolf)
    //    {
    //        SceneChangeContainer.Instance.SceneChangeToGameOver();
    //    }
    //}
}
