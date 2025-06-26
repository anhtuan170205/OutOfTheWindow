using UnityEngine;
using System;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    [SerializeField] private InputReader inputReader;
    public static GameState CurrentGameState;
    public static event Action<GameState> OnGameStateChanged;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        inputReader.PauseEvent += HandlePause;
        SetGameState(GameState.MainMenu);
    }

    public void SetGameState(GameState newState)
    {
        if (CurrentGameState == newState)
        {
            return;
        }
        CurrentGameState = newState;
        OnGameStateChanged?.Invoke(newState);
    }

    private void HandlePause()
    {
        if (CurrentGameState == GameState.InGame)
        {
            PauseGame();
        }
        else if (CurrentGameState == GameState.Paused)
        {
            ResumeGame();
        }
    }

    public void StartGame()
    {
        SetGameState(GameState.InGame);
        Time.timeScale = 1f;
        SceneLoader.LoadGame();
    }

    public void LoadMainMenu()
    {
        SetGameState(GameState.MainMenu);
        Time.timeScale = 1f;
        SceneLoader.LoadMainMenu();
    }

    public void PauseGame()
    {
        SetGameState(GameState.Paused);
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        SetGameState(GameState.InGame);
        Time.timeScale = 1f;
    }

    public void QuitGame()
    {
        SceneLoader.QuitGame();
    }
}
