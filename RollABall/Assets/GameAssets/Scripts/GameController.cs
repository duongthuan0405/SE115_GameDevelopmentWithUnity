using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField] GameObject notificationText;
    private bool isResuming;
    private void OnStart()
    {
        isResuming = false;
    }
    private void OnRestart()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
        Time.timeScale = 1.0f;  
    }

    private void OnPauseResume()
    {
        bool oldStateIsResuming = isResuming;   
        isResuming = !isResuming;
        if (oldStateIsResuming)
        {
            Time.timeScale = 1.0f;
            notificationText.SetActive(false);
        }
        else
        {
            Time.timeScale = 0.0f;
            notificationText.GetComponentInChildren<TextMeshProUGUI>().text = "Pausing";
            notificationText.SetActive(true);
        }
    }

    private void OnQuit()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
