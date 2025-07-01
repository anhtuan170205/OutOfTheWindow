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
    [SerializeField] private TextMeshProUGUI moneyText;
    private float maxShield = 100f;
    private float maxHealth = 100f;

    private void OnEnable()
    {
        Weapon.OnClipAmmoChanged += UpdateAmmoClip;
        Weapon.OnAmmoChanged += UpdateAmmo;
        Health.OnPlayerHealthChanged += UpdateHealth;
        Health.OnPlayerMaxHealthChanged += (maxHealth) => { this.maxHealth = maxHealth; };
        TurnManager.OnTurnChanged += UpdateDay;
        DayNightManager.OnStateChanged += UpdateTime;
        EnemySpawner.OnEnemyCountChanged += UpdateEnemyCount;
        Shield.OnShieldChanged += UpdateShield;
        Shield.OnMaxShieldChanged += (maxShield) => { this.maxShield = maxShield; };
        MoneyWallet.OnMoneyChanged += UpdateMoney;
    }

    private void OnDisable()
    {
        Weapon.OnClipAmmoChanged -= UpdateAmmoClip;
        Weapon.OnAmmoChanged -= UpdateAmmo;
        Health.OnPlayerHealthChanged -= UpdateHealth;
        Health.OnPlayerMaxHealthChanged -= (maxHealth) => { this.maxHealth = maxHealth; };
        TurnManager.OnTurnChanged -= UpdateDay;
        DayNightManager.OnStateChanged -= UpdateTime;
        EnemySpawner.OnEnemyCountChanged -= UpdateEnemyCount;
        Shield.OnShieldChanged -= UpdateShield;
        Shield.OnMaxShieldChanged -= (maxShield) => { this.maxShield = maxShield; };
        MoneyWallet.OnMoneyChanged -= UpdateMoney;
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
        healthSlider.value = health / maxHealth;
        healthText.text = health.ToString("000");
    }

    private void UpdateShield(int shield)
    {
        shieldSlider.value = shield / maxShield;
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

    private void UpdateMoney(int money)
    {
        moneyText.text = "MONEY : " + money.ToString("0000") + " $";
    }
}
