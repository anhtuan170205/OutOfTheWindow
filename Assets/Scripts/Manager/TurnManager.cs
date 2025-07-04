using UnityEngine;
using System;
using System.Collections;

public class TurnManager : SingletonMonoBehaviour<TurnManager>
{
    public event Action<int> OnTurnChanged;
    public event Action<int> OnDayTimerChanged;
    [SerializeField] private int baseEnemyCount = 5;
    [SerializeField] private float difficultyMultiplier = 1.2f;
    [SerializeField] private float dayDuration = 30f;
    private int currentTurn = 1;
    public int CurrentTurn
    {
        get => currentTurn;
        private set
        {
            currentTurn = value;
            OnTurnChanged?.Invoke(currentTurn);
        }
    }
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
        StartCoroutine(WaitForEnemySpawnerAndBind());
    }

    private System.Collections.IEnumerator WaitForEnemySpawnerAndBind()
    {
        yield return new WaitUntil(() => EnemySpawner.Instance != null);
        EnemySpawner.Instance.OnEveryEnemyDied += HandleEveryEnemyDied;
    }

    private void OnDisable()
    {
        EnemySpawner.Instance.OnEveryEnemyDied -= HandleEveryEnemyDied;
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
        CurrentTurn = turn;
    }

}
