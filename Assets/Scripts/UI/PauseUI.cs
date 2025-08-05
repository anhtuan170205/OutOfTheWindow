using UnityEngine;

public class PauseUI : MonoBehaviour
{
    [SerializeField] private InputReader inputReader;
    [SerializeField] private GameObject pausePanel;

    private void OnEnable()
    {
        inputReader.PauseEvent += HandlePause;
    }

    private void OnDisable()
    {
        inputReader.PauseEvent -= HandlePause;
    }

    private void HandlePause()
    {
        Debug.Log("Pause Event Triggered");
        if (BootstrappedData.Instance.GameManager.CurrentGameState == GameState.InGame)
        {
            BootstrappedData.Instance.GameManager.SetGameState(GameState.Paused);
            Debug.Log("Game Paused");
            Time.timeScale = 0f;
            pausePanel.SetActive(true);
        }
        else if (BootstrappedData.Instance.GameManager.CurrentGameState == GameState.Paused)
        {
            BootstrappedData.Instance.GameManager.SetGameState(GameState.InGame);
            Debug.Log("Game Resumed");
            Time.timeScale = 1f;
            pausePanel.SetActive(false);
        }
    }
}
