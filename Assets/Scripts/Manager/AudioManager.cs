using UnityEngine;
using System.Collections;

public class AudioManager : SingletonMonoBehaviour<AudioManager>
{
    [Header("Resources")]
    [SerializeField] private AudioClip backgroundMainMenuMusic;
    [SerializeField] private AudioClip backgroundNightMusic;
    [SerializeField] private AudioClip backgroundDayMusic;
    [SerializeField] private AudioClip shootSound;
    [SerializeField] private AudioClip reloadSound;
    [SerializeField] private AudioClip explosionSound;
    [SerializeField] private AudioClip levelCompleteSound;
    [SerializeField] private AudioClip gameOverSound;
    [SerializeField] private AudioClip dashSound;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource musicSource;

    private bool isPlayerSubscribed = false;
    private Coroutine playerSubscribeCoroutine;

    private GameManager gameManager;
    private EnemySpawner enemySpawner;
    private DayNightManager dayNightManager;
    private Weapon currentWeapon;
    private Health currentHealth;

    private void OnEnable()
    {
        gameManager = BootstrappedData.Instance.GameManager;
        enemySpawner = BootstrappedData.Instance.TurnManager.EnemySpawner;
        dayNightManager = BootstrappedData.Instance.TurnManager.DayNightManager;

        gameManager.OnGameStateChanged += HandleGameStateChanged;
        enemySpawner.OnEveryEnemyDied += PlayLevelCompleteSound;
        dayNightManager.OnDayNightStateChanged += HandleDayNightStateChanged;
        Enemy.OnAnyEnemyDied += PlayExplosionSound;
        PlayerController.OnDashStarted += PlayDashSound;

        HandleGameStateChanged(gameManager.CurrentGameState);
    }

    private void OnDisable()
    {
        gameManager.OnGameStateChanged -= HandleGameStateChanged;
        enemySpawner.OnEveryEnemyDied -= PlayLevelCompleteSound;
        dayNightManager.OnDayNightStateChanged -= HandleDayNightStateChanged;
        Enemy.OnAnyEnemyDied -= PlayExplosionSound;
        PlayerController.OnDashStarted -= PlayDashSound;

        UnsubscribeFromPlayer();
    }

    private void HandleGameStateChanged(GameState newState)
    {
        switch (newState)
        {
            case GameState.MainMenu:
                PlayBackgroundMusic(backgroundMainMenuMusic);
                UnsubscribeFromPlayer();
                break;

            case GameState.InGame:
                SubscribeToPlayer();
                PlayBackgroundMusic(dayNightManager.CurrentState == DayNightState.Day
                    ? backgroundDayMusic
                    : backgroundNightMusic);
                break;

            case GameState.GameOver:
                PlayGameOverSound();
                UnsubscribeFromPlayer();
                break;

            default:
                UnsubscribeFromPlayer();
                break;
        }
    }

    private void HandleDayNightStateChanged(DayNightState state)
    {
        if (gameManager.CurrentGameState == GameState.InGame)
        {
            PlayBackgroundMusic(state == DayNightState.Day ? backgroundDayMusic : backgroundNightMusic);
        }
    }

    private void SubscribeToPlayer()
    {
        if (isPlayerSubscribed || playerSubscribeCoroutine != null) return;
        playerSubscribeCoroutine = StartCoroutine(WaitAndSubscribeToPlayer());
    }

    private IEnumerator WaitAndSubscribeToPlayer()
    {
        yield return new WaitUntil(() =>
            Player.Instance != null &&
            Player.Instance.GetActiveWeapon()?.GetCurrentWeapon() != null &&
            Player.Instance.GetHealth() != null
        );

        currentWeapon = Player.Instance.GetActiveWeapon().GetCurrentWeapon();
        currentHealth = Player.Instance.GetHealth();

        currentWeapon.OnShoot += PlayShootSound;
        currentHealth.OnPlayerDied += PlayGameOverSound;

        isPlayerSubscribed = true;
        playerSubscribeCoroutine = null;
    }

    private void UnsubscribeFromPlayer()
    {
        if (!isPlayerSubscribed) return;

        if (currentWeapon != null)
            currentWeapon.OnShoot -= PlayShootSound;

        if (currentHealth != null)
            currentHealth.OnPlayerDied -= PlayGameOverSound;

        currentWeapon = null;
        currentHealth = null;
        isPlayerSubscribed = false;

        if (playerSubscribeCoroutine != null)
        {
            StopCoroutine(playerSubscribeCoroutine);
            playerSubscribeCoroutine = null;
        }
    }

    private void PlayBackgroundMusic(AudioClip clip)
    {
        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.volume = 0.25f;
        musicSource.Play();
    }

    private void PlayShootSound() => PlaySFX(shootSound);
    private void PlayReloadSound() => PlaySFX(reloadSound);
    private void PlayExplosionSound(Enemy _) => PlaySFX(explosionSound);
    private void PlayGameOverSound() => PlaySFX(gameOverSound);
    private void PlayLevelCompleteSound() => PlaySFX(levelCompleteSound);
    private void PlayDashSound() => PlaySFX(dashSound);

    private void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }
}
