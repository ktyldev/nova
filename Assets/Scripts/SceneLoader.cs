using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void PlayGame()
    {
        Game.Instance?.ClearScene();

        SceneManager.LoadScene("Main");
        Time.timeScale = 1.0f;
    }

    public void MainMenu()
    {
        Game.Instance.ClearScene();

        SceneManager.LoadScene("Lobby");
        Time.timeScale = 0;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
