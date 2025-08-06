using UnityEngine;

public class CursorLock : MonoBehaviour
{
    private GameManager gameManager;
    private void Start()
    {
        gameManager = BootstrappedData.Instance.GameManager;
        gameManager.OnGameStateChanged += HandleGameStateChanged;
        UpdateCursorState(gameManager.CurrentGameState);
    }

    private void OnDestroy()
    {
        gameManager.OnGameStateChanged -= HandleGameStateChanged;
    }

    private void HandleGameStateChanged(GameState newState)
    {
        UpdateCursorState(newState);
    }

    private void UpdateCursorState(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.InGame:
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                break;
            case GameState.Bootstrap:
            case GameState.MainMenu:
            case GameState.Paused:
            case GameState.GameOver:
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                break;
        }
    }
}
