using UnityEngine;


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
        GameManager.OnGameStateChanged += HandleGameStateChanged;
        Player.Instance.GetActiveWeapon().GetCurrentWeapon().OnShoot += PlayShootSound;
        Player.Instance.GetHealth().OnPlayerDied += PlayGameOverSound;
    }

    private void OnDisable()
    {
        GameManager.OnGameStateChanged -= HandleGameStateChanged;
        Player.Instance.GetActiveWeapon().GetCurrentWeapon().OnShoot -= PlayShootSound;
        Player.Instance.GetHealth().OnPlayerDied -= PlayGameOverSound;
    }

    private void HandleGameStateChanged(GameState newState)
    {

    }

    private void PlayShootSound()
    {

    }

    private void PlayReloadSound()
    {

    }
    

    private void PlayGameOverSound()
    {

    }

}