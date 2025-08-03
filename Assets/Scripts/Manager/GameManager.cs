using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    [SerializeField] private InputReader inputReader;

    [SerializeField] private GameState currentGameState = GameState.MainMenu;
    public GameState CurrentGameState => currentGameState;

    public event Action<GameState> OnGameStateChanged;

    private void Awake()
    {
        if (inputReader != null)
        {
            inputReader.PauseEvent += HandlePause;
        }
    }

    private void OnDestroy()
    {
        if (inputReader != null)
        {
            inputReader.PauseEvent -= HandlePause;
        }
    }

    public void SetGameState(GameState newState)
    {
        if (currentGameState == newState)
            return;

        currentGameState = newState;
        OnGameStateChanged?.Invoke(newState);
    }

    private void HandlePause()
    {
        if (currentGameState == GameState.InGame)
        {
            PauseGame();
        }
        else if (currentGameState == GameState.Paused)
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
