using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI ammoClipText;
    [SerializeField] private TextMeshProUGUI ammoText;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private Slider shieldSlider;
    [SerializeField] private TextMeshProUGUI shieldText;
    [SerializeField] private TextMeshProUGUI dayText;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI enemyCountText;

    private void OnEnable()
    {
        Weapon.OnClipAmmoChanged += UpdateAmmoClip;
        Weapon.OnAmmoChanged += UpdateAmmo;
        Health.OnPlayerHealthChanged += UpdateHealth;
        TurnManager.OnTurnChanged += UpdateDay;
        DayNightManager.OnStateChanged += UpdateTime;
        EnemySpawner.OnEnemyCountChanged += UpdateEnemyCount;
        Shield.OnShieldChanged += UpdateShield;
    }

    private void OnDisable()
    {
        Weapon.OnClipAmmoChanged -= UpdateAmmoClip;
        Weapon.OnAmmoChanged -= UpdateAmmo;
        Health.OnPlayerHealthChanged -= UpdateHealth;
        TurnManager.OnTurnChanged -= UpdateDay;
        DayNightManager.OnStateChanged -= UpdateTime;
        EnemySpawner.OnEnemyCountChanged -= UpdateEnemyCount;
        Shield.OnShieldChanged -= UpdateShield;
    }

    private void UpdateAmmoClip(int ammoClip)
    {
        ammoClipText.text = ammoClip.ToString("00");
    }

    private void UpdateAmmo(int ammo)
    {
        ammoText.text = "/" + ammo.ToString("000");
    }

    private void UpdateHealth(int health)
    {
        healthSlider.value = health / 100f;
        healthText.text = health.ToString("000");
    }

    private void UpdateShield(int shield)
    {
        shieldSlider.value = shield / 100f;
        shieldText.text = shield.ToString("000");
    }

    private void UpdateDay(int day)
    {
        dayText.text = "Day " + day.ToString();
    }

    private void UpdateTime(DayNightState state)
    {
        timeText.text = state == DayNightState.Day ? "DAY" : "NIGHT";
    }

    private void UpdateEnemyCount(int count)
    {
        if (count == 0)
        {
            enemyCountText.text = "PREPARING FOR THE NEXT NIGHT";
        }
        else
        {
            enemyCountText.text = "REMAINING ENEMIES : " + count.ToString("00");
        }
    }
}
