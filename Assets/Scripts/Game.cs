using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    public static Game Instance { get; private set; }

    public GameObject shipExplosion;
    public GameObject bulletExplosion;
    public Transform bulletParent;
    private Ship _ship;
    public Ship Ship
    {
        get
        {
            if (_ship == null)
            {
                _ship = GetComponentInChildren<Ship>();
            }

            return _ship;
        }
    }

    private void Awake()
    {
        if (Instance != null)
            throw new System.Exception();

        Instance = this;
    }

    public void GameOver()
    {
        Time.timeScale = 0f;
        Restart(3f);
    }

    private void Restart(float delay = 0) => Instance.StartCoroutine(RestartScene(delay));
    private IEnumerator RestartScene(float delay)
    {
        var currentScene = SceneManager.GetActiveScene();

        yield return new WaitForSecondsRealtime(delay);

        Instance = null;
        SceneManager.LoadScene(currentScene.buildIndex);
        Time.timeScale = 1.0f;
    }
}
