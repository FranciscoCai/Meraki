using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
    public GameObject menuUI;
    public UnityEngine.UI.Button resumeButton;
    public UnityEngine.UI.Button quitButton;

    private UnityEngine.UI.Button[] buttons;
    private int currentIndex = 0;

    void Start()
    {
        buttons = new UnityEngine.UI.Button[] { resumeButton, quitButton };
        menuUI.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleMenu();
        }

        if (menuUI.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                currentIndex = (currentIndex - 1 + buttons.Length) % buttons.Length;
                UpdateSelection();
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                currentIndex = (currentIndex + 1) % buttons.Length;
                UpdateSelection();
            }

            if (Input.GetKeyDown(KeyCode.Return))
            {
                buttons[currentIndex].onClick.Invoke();
            }
        }
    }

    void ToggleMenu()
    {
        bool isPaused = !menuUI.activeSelf;
        menuUI.SetActive(isPaused);
        Time.timeScale = isPaused ? 0f : 1f;
        currentIndex = 0;
        UpdateSelection();
    }

    void UpdateSelection()
    {
        EventSystem.current.SetSelectedGameObject(buttons[currentIndex].gameObject);
    }

    public void ResumeGame()
    {
        ToggleMenu();
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}