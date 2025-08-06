using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{
    private static GameState nextGameState = GameState.Bootstrap;
    private GameState currentGameState;
    public GameState CurrentGameState => currentGameState;
    public event Action<GameState> OnGameStateChanged;

    private void Awake()
    {
        currentGameState = GameState.Bootstrap;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        switch (scene.name)
        {
            case "MainMenu":
                SetGameState(GameState.MainMenu);
                break;

            case "Game":
                if (nextGameState == GameState.InGame)
                {
                    SetGameState(GameState.InGame);
                }
                break;

            case "GameOver":
                SetGameState(GameState.GameOver);
                break;

            case "Bootstrap":
                SetGameState(GameState.Bootstrap);
                break;
        }
    }

    public void SetGameState(GameState newState)
    {
        if (currentGameState == newState)
            return;

        currentGameState = newState;
        OnGameStateChanged?.Invoke(newState);
    }

    public void StartGame()
    {
        Time.timeScale = 1f;
        nextGameState = GameState.InGame;
        SceneLoader.LoadGame();
    }

    public void LoadMainMenu()
    {
        nextGameState = GameState.MainMenu;
        SceneLoader.LoadMainMenu();
    }

    public void QuitGame()
    {
        SceneLoader.QuitGame();
    }

    private void Update()
    {
        Debug.Log($"[GameManager] Current Game State: {currentGameState}");
    }

    public void GameOver()
    {
        nextGameState = GameState.GameOver;
        SceneLoader.LoadGameOver();
    }
}
