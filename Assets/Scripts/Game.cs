using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    public static Game Instance { get; private set; }

    public GameObject shipExplosion;
    public GameObject bulletExplosion;
    public GameObject gameOverUI;
    public Transform bulletParent;
    public GameObject healthPickup;
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
    public SoundEngine Audio { get; private set; }

    private void Awake()
    {
        if (Instance != null)
            throw new System.Exception();

        Instance = this;

        Audio = GameObject.Find(GameConstants.Audio)
            .GetComponent<SoundEngine>();
    }

    public bool IsGameOver { get; private set; }
    public UnityEvent OnGameOver { get; private set; } = new UnityEvent();
    public static bool IsRunning => Instance != null && !Instance.IsGameOver;
    public bool IsPaused { get; set; }
    public void GameOver()
    {
        IsGameOver = true;
        OnGameOver.Invoke();

        gameOverUI.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Restart()
    {
        IsGameOver = true;
        gameOverUI.SetActive(false);
        Restart(0);
    }

    private void Restart(float delay)
    {
        Instance.StartCoroutine(RestartScene(delay));
    }
    private IEnumerator RestartScene(float delay)
    {
        var currentScene = SceneManager.GetActiveScene();

        yield return new WaitForSecondsRealtime(delay);

        SceneManager.LoadScene(currentScene.buildIndex);
        Time.timeScale = 1.0f;
    }

    public void ClearScene()
    {
        MapGeneration.Instance?.ClearChunks();
        GameObject.Destroy(MapGeneration.Instance.gameObject);

        Game.Instance = null;
    }
}
