using UnityEngine;
using System.Collections;

public class AudioManager : SingletonMonoBehaviour<AudioManager>
{
    [Header("Resources")]
    [SerializeField] private AudioClip backgroundNightMusic;
    [SerializeField] private AudioClip backgroundDayMusic;
    [SerializeField] private AudioClip shootSound;
    [SerializeField] private AudioClip reloadSound;
    [SerializeField] private AudioClip explosionSound;
    [SerializeField] private AudioClip levelCompleteSound;
    [SerializeField] private AudioClip gameOverSound;

    // [Header("Settings")]
    // [Range(0, 1)][SerializeField] private float backgroundVolume = 0.5f;
    // [Range(0, 1)][SerializeField] private float soundEffectVolume = 1.0f;
    // [Range(1, 100)][SerializeField] private float gameOverVolume = 50f;

    private void OnEnable()
    {
        StartCoroutine(WaitForPlayerAndBind());
    }

    IEnumerator WaitForPlayerAndBind()
    {
        yield return new WaitUntil(() =>
            Player.Instance != null &&
            Player.Instance.GetActiveWeapon() != null &&
            Player.Instance.GetActiveWeapon().GetCurrentWeapon() != null &&
            Player.Instance.GetHealth() != null &&
            EnemySpawner.Instance != null
        );

        GameManager.OnGameStateChanged += HandleGameStateChanged;
        Player.Instance.GetActiveWeapon().GetCurrentWeapon().OnShoot += PlayShootSound;
        Player.Instance.GetHealth().OnPlayerDied += PlayGameOverSound;
        EnemySpawner.Instance.OnEveryEnemyDied += PlayLevelCompleteSound;
        Enemy.OnAnyEnemyDied += PlayExplosionSound;
    }

    private void OnDisable()
    {
        GameManager.OnGameStateChanged -= HandleGameStateChanged;
        Player.Instance.GetActiveWeapon().GetCurrentWeapon().OnShoot -= PlayShootSound;
        Player.Instance.GetHealth().OnPlayerDied -= PlayGameOverSound;
        EnemySpawner.Instance.OnEveryEnemyDied -= PlayLevelCompleteSound;
        Enemy.OnAnyEnemyDied -= PlayExplosionSound;
    }

    private void HandleGameStateChanged(GameState newState)
    {
        switch (newState)
        {
            case GameState.GameOver:
                PlayGameOverSound();
                break;
            case GameState.InGame:
                if (DayNightManager.Instance.CurrentState == DayNightState.Day)
                {
                    AudioSource.PlayClipAtPoint(backgroundDayMusic, Vector3.zero);
                }
                else
                {
                    AudioSource.PlayClipAtPoint(backgroundNightMusic, Vector3.zero);
                }
                break;
        }
    }

    private void PlayShootSound()
    {
        AudioSource.PlayClipAtPoint(shootSound, Vector3.zero);
    }

    private void PlayReloadSound()
    {
        AudioSource.PlayClipAtPoint(reloadSound, Vector3.zero);
    }

    private void PlayExplosionSound(Enemy enemy)
    {
        AudioSource.PlayClipAtPoint(explosionSound, enemy.transform.position);
    }

    private void PlayGameOverSound()
    {
        AudioSource.PlayClipAtPoint(gameOverSound, Vector3.zero);
    }

    private void PlayLevelCompleteSound()
    {
        AudioSource.PlayClipAtPoint(levelCompleteSound, Vector3.zero);
    }
}