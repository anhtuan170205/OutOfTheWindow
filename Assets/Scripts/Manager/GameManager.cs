using UnityEngine;
using System;
using System.Collections;

public class GameManager : MonoBehaviour
{
    private GameState currentGameState = GameState.MainMenu;
    public GameState CurrentGameState => currentGameState;

    public event Action<GameState> OnGameStateChanged;
    public void SetGameState(GameState newState)
    {
        if (currentGameState == newState)
            return;

        currentGameState = newState;
        OnGameStateChanged?.Invoke(newState);
    }

    private void Awake()
    {
        Debug.Log($"[GameManager] Awake in scene: {UnityEngine.SceneManagement.SceneManager.GetActiveScene().name}");

        if (BootstrappedData.Instance.GameManager != this)
        {
            Debug.LogWarning("[GameManager] This instance is not the Bootstrapped one. Destroying.");
            Destroy(gameObject);
            return;
        }

        Debug.Log("[GameManager] Bootstrapped instance confirmed.");
    }


    public void StartGame()
    {
        Debug.Log("Starting Game");
        SetGameState(GameState.InGame);
        Time.timeScale = 1f;
        BootstrappedData.Instance.StartCoroutine(DelayedGameLoad());
    }

    private IEnumerator DelayedGameLoad()
    {
        yield return null;
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

    private GameState lastLoggedState;

    private void Update()
    {
        if (currentGameState != lastLoggedState)
        {
            Debug.Log($"[GameManager] Game State changed to: {currentGameState}");
            lastLoggedState = currentGameState;
        }
    }

}
