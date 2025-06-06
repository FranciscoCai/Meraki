using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneChangeContainer : MonoBehaviour
{
    public static SceneChangeContainer Instance { get; private set; }
    public bool franPuta = false;
    public bool gameOver = false;
    public Animator transition;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    private void Update()
    {
        if (Input.anyKeyDown && franPuta && gameOver) 
        {
            SceneManager.LoadScene("TutorialPart2");
        }
        if (Input.anyKeyDown && !franPuta & gameOver)
        {
            SceneManager.LoadScene("FranciscoIluminacion");
        }
    }
    public void SceneChangeToMain()
    {
       // StartCoroutine(LoadLevel());
    }
    public void SceneChangeToVictory()
    {
        SceneManager.LoadScene("Victory");
    }
    public void SceneChangeToGameOver()
    {
        if(franPuta)
        {
            SceneManager.LoadScene("GameOver");
        }
        if (!franPuta)
        {
            SceneManager.LoadScene("GameOver 2");
        }

    }
    /*IEnumerator LoadLevel()
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(1);

       SceneManager.LoadScene("TutorialPart2");
    }*/
}
