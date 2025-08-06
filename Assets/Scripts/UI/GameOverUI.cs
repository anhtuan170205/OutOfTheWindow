using UnityEngine;
using TMPro;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    private GameManager gameManager => BootstrappedData.Instance.GameManager;
    private TurnManager turnManager => BootstrappedData.Instance.TurnManager;

    private void OnEnable()
    {
        gameManager.OnGameStateChanged += HandleGameStateChanged;
    }

    private void OnDisable()
    {
        gameManager.OnGameStateChanged -= HandleGameStateChanged;
    }

    private void HandleGameStateChanged(GameState newState)
    {
        if (newState == GameState.GameOver)
        {
            DisplayGameOver();
        }
    }

    private void DisplayGameOver()
    {
        scoreText.text = $"Your have survived {turnManager.CurrentTurn - 1} nights!";
    }
}
