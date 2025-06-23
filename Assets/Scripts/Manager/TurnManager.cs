using UnityEngine;

public class TurnManager : SingletonMonoBehaviour<TurnManager>
{
    [SerializeField] private int baseEnemyCount = 5;
    [SerializeField] private float difficultyMultiplier = 1.2f;
    private int currentTurn = 0;
    public int CurrentTurn => currentTurn;

    public void NextTurn()
    {
        currentTurn++;
        SetNight();
    }

    public int GetEnemyForCurrentTurn()
    {
        int enemyCount = Mathf.CeilToInt(baseEnemyCount * Mathf.Pow(difficultyMultiplier, currentTurn));
        return enemyCount;
    }

    public void ResetTurn()
    {
        currentTurn = 0;
        SetNight();
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
        SetDay();
        Debug.Log($"All enemies defeated! Setting to day. Current turn: {currentTurn}");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            NextTurn();
            Debug.Log($"Next turn: {currentTurn}. Enemies for this turn: {GetEnemyForCurrentTurn()}");
        }
    }

    private void SetDay()
    {
        DayNightManager.Instance.SetState(DayNightState.Day);
    }

    private void SetNight()
    {
        DayNightManager.Instance.SetState(DayNightState.Night);
        EnemySpawner.Instance.StartSpawning(GetEnemyForCurrentTurn());
    }
}
