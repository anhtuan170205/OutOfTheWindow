using UnityEngine;
using System;

public class TurnManager : MonoBehaviour
{
    public event Action<int> OnTurnChanged;
    public event Action<int> OnDayTimerChanged;

    public DayNightManager DayNightManager { get; private set; }
    public LightingManager LightingManager { get; private set; }
    public EnemySpawner EnemySpawner { get; private set; }

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

        DayNightManager = GetComponentInChildren<DayNightManager>(true);
        LightingManager = GetComponentInChildren<LightingManager>(true);
        EnemySpawner = GetComponentInChildren<EnemySpawner>(true);
    }

    private void OnEnable()
    {
        EnemySpawner.OnEveryEnemyDied += HandleEveryEnemyDied;
        gameManager.OnGameStateChanged += HandleGameStateChanged;
    }

    private void OnDisable()
    {
        EnemySpawner.OnEveryEnemyDied -= HandleEveryEnemyDied;
        gameManager.OnGameStateChanged -= HandleGameStateChanged;
    }

    private void Start()
    {
        if (gameManager.CurrentGameState == GameState.InGame)
        {
            isActive = true;
            ResetTurn();
        }
    }

    private void Update()
    {
        if (!isActive) return;

        if (DayNightManager.CurrentState == DayNightState.Day)
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
        DayNightManager.SetState(DayNightState.Day);
        LightingManager.UpdateLighting(DayNightState.Day);
        dayTimer = dayDuration;
    }

    private void SetNight()
    {
        DayNightManager.SetState(DayNightState.Night);
        LightingManager.UpdateLighting(DayNightState.Night);
        EnemySpawner.StartSpawning(GetEnemyForCurrentTurn());
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

        if (isActive && !wasPaused)
        {
            ResetTurn();
        }
    }
}
