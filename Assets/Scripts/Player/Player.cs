using UnityEngine;

public class Player : SingletonMonoBehaviour<Player>
{
    [SerializeField] private Health health;

    private void OnEnable()
    {
        health.OnPlayerDied += HandlePlayerDied;
    }

    private void OnDisable()
    {
        health.OnPlayerDied -= HandlePlayerDied;
    }
    private void HandlePlayerDied()
    {
        Debug.Log("Player Died");
    }
}