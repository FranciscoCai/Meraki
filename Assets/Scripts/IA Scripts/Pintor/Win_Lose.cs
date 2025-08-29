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
            if(SpawnManager.Instance != null)
            {
                if(SpawnManager.Instance.SpawnCount<= 0)
                {
                    SceneManager.LoadScene(SceneToChange);
                }
                else
                {
                    SpawnManager.Instance.ActiveSpawn();
                    GameManager.Instance.StopChangeTurn();
                }
            }

        }
    }

}
