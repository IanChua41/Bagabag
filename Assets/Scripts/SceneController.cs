using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            //Destroy(gameObject);
        }
    }

    public void MainMenu()
    {
        SceneManager.LoadSceneAsync(0);
    }

    public void NextLevel()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void TutorialLevel()
    {
        SceneManager.LoadScene(1);
    }

    public void Level1()
    {
        SceneManager.LoadSceneAsync(2);
    }

    public void Level2()
    {
        SceneManager.LoadSceneAsync(3);
    }

    public void Level3()
    {
        SceneManager.LoadSceneAsync(4);
    }
    public void Credits()
    {
        SceneManager.LoadSceneAsync(5);
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadSceneAsync(sceneName);
    }

    public void QuitButton()
    {
        /*Debug.LogWarning("Play mode editor closed");

        if (Application.isEditor)
        {
            UnityEditor.EditorApplication.isPlaying = false;
        }*/
        // Use this only when game is ready to be built into a .exe file
        Application.Quit();
    }
}
