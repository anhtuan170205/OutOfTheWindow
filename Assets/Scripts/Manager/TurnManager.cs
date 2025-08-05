using UnityEngine;
using System;

public class TurnManager : MonoBehaviour
{
    public event Action<int> OnTurnChanged;
    public event Action<int> OnDayTimerChanged;

    [Header("Config")]
    [SerializeField] private int baseEnemyCount = 5;
    [SerializeField] private float difficultyMultiplier = 1.2f;
    [SerializeField] private float dayDuration = 30f;

    [Header("References")]
    [SerializeField] private DayNightManager dayNightManager;
    [SerializeField] private LightingManager lightingManager;
    [SerializeField] private EnemySpawner enemySpawner;

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

    private void OnEnable()
    {
        enemySpawner.OnEveryEnemyDied += HandleEveryEnemyDied;
    }

    private void OnDisable()
    {
        enemySpawner.OnEveryEnemyDied -= HandleEveryEnemyDied;
    }

    private void Start()
    {
        ResetTurn();
    }

    private void Update()
    {
        if (dayNightManager.CurrentState == DayNightState.Day)
        {
            dayTimer -= Time.deltaTime;
            OnDayTimerChanged?.Invoke(Mathf.CeilToInt(dayTimer));

            if (dayTimer <= 0f)
            {
                Debug.Log("Day ended. Switching to night.");
                SetNight();
            }
        }
    }

    public void ResetTurn()
    {
        SetTurn(1);
        SetDay();
    }

    public void NextTurn()
    {
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
        Debug.Log("All enemies defeated. Proceeding to next turn.");
        NextTurn();
    }

    public EnemySpawner GetEnemySpawner() => enemySpawner;
    public DayNightManager GetDayNightManager() => dayNightManager;
    public LightingManager GetLightingManager() => lightingManager;
}
