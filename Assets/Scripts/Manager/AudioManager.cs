using UnityEngine;
using System.Collections;

public class AudioManager : SingletonMonoBehaviour<AudioManager>
{
    // [Header("Resources")]
    // [SerializeField] private AudioClip backgroundNightMusic;
    // [SerializeField] private AudioClip backgroundDayMusic;
    // [SerializeField] private AudioClip shootSound;
    // [SerializeField] private AudioClip reloadSound;
    // [SerializeField] private AudioClip explosionSound;
    // [SerializeField] private AudioClip levelCompleteSound;
    // [SerializeField] private AudioClip gameOverSound;
    // [SerializeField] private AudioClip dashSound;

    // [Header("Audio Sources")]
    // [SerializeField] private AudioSource sfxSource;
    // [SerializeField] private AudioSource musicSource;

    // private void OnEnable()
    // {
    //     StartCoroutine(WaitForPlayerAndBind());
    // }

    // private IEnumerator WaitForPlayerAndBind()
    // {
    //     yield return new WaitUntil(() =>
    //         Player.Instance != null &&
    //         Player.Instance.GetActiveWeapon() != null &&
    //         Player.Instance.GetActiveWeapon().GetCurrentWeapon() != null &&
    //         Player.Instance.GetHealth() != null &&
    //         EnemySpawner.Instance != null
    //     );

    //     GameManager.OnGameStateChanged += HandleGameStateChanged;
    //     Player.Instance.GetActiveWeapon().GetCurrentWeapon().OnShoot += PlayShootSound;
    //     Player.Instance.GetHealth().OnPlayerDied += PlayGameOverSound;
    //     EnemySpawner.Instance.OnEveryEnemyDied += PlayLevelCompleteSound;
    //     Enemy.OnAnyEnemyDied += PlayExplosionSound;
    //     PlayerController.OnDashPoolChanged += PlayDashSound;
    // }

    // private void OnDisable()
    // {
    //     GameManager.OnGameStateChanged -= HandleGameStateChanged;

    //     if (Player.Instance != null && Player.Instance.GetActiveWeapon()?.GetCurrentWeapon() != null)
    //         Player.Instance.GetActiveWeapon().GetCurrentWeapon().OnShoot -= PlayShootSound;

    //     if (Player.Instance?.GetHealth() != null)
    //         Player.Instance.GetHealth().OnPlayerDied -= PlayGameOverSound;

    //     if (EnemySpawner.Instance != null)
    //         EnemySpawner.Instance.OnEveryEnemyDied -= PlayLevelCompleteSound;

    //     Enemy.OnAnyEnemyDied -= PlayExplosionSound;
    //     PlayerController.OnDashPoolChanged -= PlayDashSound;
    // }

    // private void HandleGameStateChanged(GameState newState)
    // {
    //     switch (newState)
    //     {
    //         case GameState.GameOver:
    //             PlayGameOverSound();
    //             break;

    //         case GameState.InGame:
    //             PlayBackgroundMusic(DayNightManager.Instance.CurrentState == DayNightState.Day
    //                 ? backgroundDayMusic
    //                 : backgroundNightMusic);
    //             break;
    //     }
    // }

    // private void PlayBackgroundMusic(AudioClip clip)
    // {
    //     if (musicSource == null || clip == null) return;

    //     musicSource.clip = clip;
    //     musicSource.loop = true;
    //     musicSource.Play();
    // }

    // private void PlayShootSound() => PlaySFX(shootSound);
    // private void PlayReloadSound() => PlaySFX(reloadSound);
    // private void PlayExplosionSound(Enemy enemy) => PlaySFX(explosionSound);
    // private void PlayGameOverSound() => PlaySFX(gameOverSound);
    // private void PlayLevelCompleteSound() => PlaySFX(levelCompleteSound);
    // private void PlayDashSound(float currentDashPool) => PlaySFX(dashSound);

    // private void PlaySFX(AudioClip clip)
    // {
    //     if (sfxSource == null || clip == null) return;
    //     sfxSource.PlayOneShot(clip);
    // }
}
