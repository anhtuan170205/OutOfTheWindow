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
        if (BootstrappedData.Instance.GameManager.CurrentGameState == GameState.InGame)
        {
            PauseGame();
        }
        else if (BootstrappedData.Instance.GameManager.CurrentGameState == GameState.Paused)
        {
            ResumeGame();
        }
    }

    public void PauseGame()
    {
        BootstrappedData.Instance.GameManager.SetGameState(GameState.Paused);
        Time.timeScale = 0f;
        pausePanel.SetActive(true);
    }

    public void ResumeGame()
    {
        BootstrappedData.Instance.GameManager.SetGameState(GameState.InGame);
        Time.timeScale = 1f;
        pausePanel.SetActive(false);
    }
}
