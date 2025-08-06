using UnityEngine;
using System;

public class TurnManager : MonoBehaviour
{
    public event Action<int> OnTurnChanged;
    public event Action<int> OnDayTimerChanged;

    [Header("References")]
    [SerializeField] private DayNightManager dayNightManager;
    [SerializeField] private LightingManager lightingManager;
    [SerializeField] private EnemySpawner enemySpawner;

    [Header("Settings")]
    [SerializeField] private int baseEnemyCount = 10;
    [SerializeField] private float difficultyMultiplier = 1.2f;
    [SerializeField] private float dayDuration = 15f;

    private GameManager gameManager;
    private GameState previousState = GameState.Bootstrap;
    private bool isActive = false;

    private int currentTurn = 1;
    private float dayTimer = 0f;

    public int CurrentTurn
    {
        get => currentTurn;
        private set
        {
            currentTurn = value;
            OnTurnChanged?.Invoke(currentTurn);
        }
    }

    private void Awake()
    {
        gameManager = BootstrappedData.Instance.GameManager;
    }

    private void OnEnable()
    {
        enemySpawner.OnEveryEnemyDied += HandleEveryEnemyDied;
        gameManager.OnGameStateChanged += HandleGameStateChanged;
    }

    private void OnDisable()
    {
        enemySpawner.OnEveryEnemyDied -= HandleEveryEnemyDied;
        gameManager.OnGameStateChanged -= HandleGameStateChanged;
    }

    private void Start()
    {
        if (gameManager == null)
        {
            Debug.LogError("GameManager is not initialized in TurnManager.");
            return;
        }
        if (gameManager != null && gameManager.CurrentGameState == GameState.InGame)
        {
            isActive = true;
            ResetTurn();
        }
    }

    private void Update()
    {
        if (!isActive) return;

        if (dayNightManager.CurrentState == DayNightState.Day)
        {
            dayTimer -= Time.deltaTime;
            OnDayTimerChanged?.Invoke(Mathf.CeilToInt(dayTimer));

            if (dayTimer <= 0f)
            {
                SetNight();
            }
        }
    }

    public void ResetTurn()
    {
        if (!isActive) return;
        SetTurn(1);
        SetDay();
    }

    public void NextTurn()
    {
        if (!isActive) return;
        SetTurn(currentTurn + 1);
        SetDay();
    }

    public int GetEnemyForCurrentTurn()
    {
        return Mathf.CeilToInt(baseEnemyCount * Mathf.Pow(difficultyMultiplier, currentTurn));
    }

    public void SetTurn(int turn)
    {
        CurrentTurn = turn;
    }

    private void SetDay()
    {
        dayNightManager.SetState(DayNightState.Day);
        lightingManager.UpdateLighting(DayNightState.Day);
        dayTimer = dayDuration;
    }

    private void SetNight()
    {
        dayNightManager.SetState(DayNightState.Night);
        lightingManager.UpdateLighting(DayNightState.Night);
        enemySpawner.StartSpawning(GetEnemyForCurrentTurn());
    }

    private void HandleEveryEnemyDied()
    {
        if (!isActive) return;
        NextTurn();
    }

    private void HandleGameStateChanged(GameState newState)
    {
        bool wasPaused = (previousState == GameState.Paused);
        previousState = newState;

        isActive = (newState == GameState.InGame);

        if (isActive)
        {
            if (!wasPaused)
            {
                ResetTurn();
            }
        }
    }

    public EnemySpawner GetEnemySpawner() => enemySpawner;
    public DayNightManager GetDayNightManager() => dayNightManager;
    public LightingManager GetLightingManager() => lightingManager;
}
