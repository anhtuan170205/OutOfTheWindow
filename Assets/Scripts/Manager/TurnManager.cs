using UnityEngine;
using System;

public class TurnManager : SingletonMonoBehaviour<TurnManager>
{
    public event Action<int> OnTurnChanged;
    public event Action<int> OnDayTimerChanged;
    [SerializeField] private int baseEnemyCount = 5;
    [SerializeField] private float difficultyMultiplier = 1.2f;
    [SerializeField] private float dayDuration = 30f;
    private int currentTurn = 1;
    private float dayTimer = 0f;
    public void NextTurn()
    {
        SetTurn(currentTurn + 1);
        SetDay();
    }


    public int GetEnemyForCurrentTurn()
    {
        int enemyCount = Mathf.CeilToInt(baseEnemyCount * Mathf.Pow(difficultyMultiplier, currentTurn));
        return enemyCount;
    }

    public void ResetTurn()
    {
        SetTurn(1);
        SetDay();
    }


    private void Start()
    {
        ResetTurn();
    }

    private void OnEnable()
    {
        EnemySpawner.OnEveryEnemyDied += HandleEveryEnemyDied;
    }

    private void OnDisable()
    {
        EnemySpawner.OnEveryEnemyDied -= HandleEveryEnemyDied;
    }

    private void HandleEveryEnemyDied()
    {
        NextTurn();
    }

    private void Update()
    {
        if (DayNightManager.Instance.CurrentState == DayNightState.Day)
        {
            dayTimer -= Time.deltaTime;
            Debug.Log($"Day Timer: {dayTimer}");
            OnDayTimerChanged?.Invoke(Mathf.CeilToInt(dayTimer));

            if (dayTimer <= 0f)
            {
                SetNight();
            }
        }
    }



    private void SetDay()
    {
        DayNightManager.Instance.SetState(DayNightState.Day);
        dayTimer = dayDuration;
    }

    private void SetNight()
    {
        DayNightManager.Instance.SetState(DayNightState.Night);
        EnemySpawner.Instance.StartSpawning(GetEnemyForCurrentTurn());
    }

    public void SetTurn(int turn)
    {
        currentTurn = turn;
        OnTurnChanged?.Invoke(currentTurn);
    }

}
