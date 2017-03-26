using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SingletonMusic : MonoBehaviour
{

    private static SingletonMusic instance;
    private enum GameState { Menu, InGame, Lose}
    private GameState _currentGameState = GameState.Menu;
    private LevelManager _levelManager;
    private AudioSource _audioSource;

    public static SingletonMusic GetInstance()
    {
        return instance;
    }

    private GameState GetGameState()
    {
        if (instance != null)
            return instance._currentGameState;
        else return GameState.Menu; // Change this
    }

    private void SetGameState(GameState newGameState)
    {
        if (instance != null)
            instance._currentGameState = newGameState;
    }

    private void Update()
    {
        bool validState = _audioSource != null && _levelManager != null;
        if (validState) {
            if (_levelManager.isGamePaused)
            {
                _audioSource.Pause();
            } else
            {
                _audioSource.UnPause();
            }
        }
    }

    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        GameObject tmp = GameObject.Find("LevelManager");
        if (tmp != null) _levelManager = tmp.GetComponent<LevelManager>();


        GameState potentiallyNewGameState;
        string currentSceneName = SceneManager.GetActiveScene().name;
        if (currentSceneName.Contains("Lose")) potentiallyNewGameState = GameState.Lose;
        else if (currentSceneName.Contains("Level")) potentiallyNewGameState = GameState.InGame;
        else potentiallyNewGameState = GameState.Menu;

        bool transitioning = instance == null || potentiallyNewGameState != instance.GetGameState();
        if (transitioning) Debug.Log("Shit is transitioning nigga.");
        else Debug.Log("NOT TRANSITIONING");
        if (!transitioning && instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else if (transitioning)
        {
            if (instance != null)
            {
                instance.GetComponent<AudioSource>().Stop();
            }
            instance = this;
            instance.SetGameState(potentiallyNewGameState);
        }
        DontDestroyOnLoad(this.gameObject);
    }

}
