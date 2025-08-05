using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Collections;

public class HUDisplay : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI ammoClipText;
    [SerializeField] private TextMeshProUGUI ammoText;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private Slider shieldSlider;
    [SerializeField] private TextMeshProUGUI shieldText;
    [SerializeField] private TextMeshProUGUI dayText;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI dayTimerText;
    [SerializeField] private TextMeshProUGUI enemyCountText;
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private List<Slider> dashSliders;

    private float maxShield = 100f;
    private float maxHealth = 100f;
    private Weapon currentBoundWeapon;

    private Player player => Player.Instance;
    private TurnManager turnManager => BootstrappedData.Instance.TurnManager;

    private void OnDisable()
    {
        if (player == null || turnManager == null) return;
        Unbind();
    }

    private IEnumerator Start()
    {
        yield return new WaitUntil(() =>
            Player.Instance != null &&
            Player.Instance.GetActiveWeapon() != null &&
            Player.Instance.GetActiveWeapon().GetCurrentWeapon() != null &&
            Player.Instance.GetHealth() != null &&
            Player.Instance.GetShield() != null &&
            Player.Instance.GetMoneyWallet() != null
        );

        Bind();

        dayTimerText.gameObject.SetActive(true);
        UpdateTime(turnManager.GetDayNightManager().CurrentState);
        UpdateEnemyCount(0);
        UpdateMoney(0);
    }

    private void Bind()
    {
        var weaponManager = player.GetActiveWeapon();
        if (weaponManager == null) return;

        currentBoundWeapon = weaponManager.GetCurrentWeapon();
        if (currentBoundWeapon == null) return;

        weaponManager.OnWeaponChanged += HandleWeaponChanged;
        currentBoundWeapon.OnClipAmmoChanged += UpdateAmmoClip;
        currentBoundWeapon.OnAmmoChanged += UpdateAmmo;

        var health = player.GetHealth();
        health.OnPlayerHealthChanged += UpdateHealth;
        health.OnPlayerMaxHealthChanged += max => maxHealth = max;

        var shield = player.GetShield();
        shield.OnShieldChanged += UpdateShield;
        shield.OnMaxShieldChanged += max => maxShield = max;

        player.GetMoneyWallet().OnMoneyChanged += UpdateMoney;

        turnManager.OnTurnChanged += UpdateDay;
        turnManager.OnDayTimerChanged += UpdateDayTimer;
        turnManager.GetDayNightManager().OnStateChanged += UpdateTime;
        turnManager.GetEnemySpawner().OnEnemyCountChanged += UpdateEnemyCount;

        PlayerController.OnDashPoolChanged += UpdateDash;

        UpdateAmmoClip(currentBoundWeapon.GetCurrentClipAmmo());
        UpdateAmmo(currentBoundWeapon.GetCurrentAmmo());
    }

    private void Unbind()
    {
        var weaponManager = player.GetActiveWeapon();
        weaponManager.OnWeaponChanged -= HandleWeaponChanged;

        if (currentBoundWeapon != null)
        {
            currentBoundWeapon.OnClipAmmoChanged -= UpdateAmmoClip;
            currentBoundWeapon.OnAmmoChanged -= UpdateAmmo;
        }

        var health = player.GetHealth();
        health.OnPlayerHealthChanged -= UpdateHealth;
        health.OnPlayerMaxHealthChanged -= max => maxHealth = max;

        var shield = player.GetShield();
        shield.OnShieldChanged -= UpdateShield;
        shield.OnMaxShieldChanged -= max => maxShield = max;

        player.GetMoneyWallet().OnMoneyChanged -= UpdateMoney;

        turnManager.OnTurnChanged -= UpdateDay;
        turnManager.OnDayTimerChanged -= UpdateDayTimer;
        turnManager.GetDayNightManager().OnStateChanged -= UpdateTime;
        turnManager.GetEnemySpawner().OnEnemyCountChanged -= UpdateEnemyCount;

        PlayerController.OnDashPoolChanged -= UpdateDash;
    }

    private void HandleWeaponChanged(Weapon newWeapon)
    {
        if (currentBoundWeapon != null)
        {
            currentBoundWeapon.OnClipAmmoChanged -= UpdateAmmoClip;
            currentBoundWeapon.OnAmmoChanged -= UpdateAmmo;
        }

        currentBoundWeapon = newWeapon;

        currentBoundWeapon.OnClipAmmoChanged += UpdateAmmoClip;
        currentBoundWeapon.OnAmmoChanged += UpdateAmmo;

        UpdateAmmoClip(currentBoundWeapon.GetCurrentClipAmmo());
        UpdateAmmo(currentBoundWeapon.GetCurrentAmmo());
    }

    private void UpdateAmmoClip(int ammoClip) =>
        ammoClipText.text = ammoClip.ToString("00");

    private void UpdateAmmo(int ammo) =>
        ammoText.text = "/" + ammo.ToString("000");

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

    private void UpdateDay(int day) =>
        dayText.text = day.ToString();

    private void UpdateDayTimer(int seconds) =>
        dayTimerText.text = $"DAY DURATION : {seconds:00}s";

    private void UpdateTime(DayNightState state)
    {
        timeText.text = state == DayNightState.Day ? "DAY" : "NIGHT";
        dayTimerText.gameObject.SetActive(state == DayNightState.Day);
    }

    private void UpdateEnemyCount(int count)
    {
        enemyCountText.text = count == 0
            ? "PRESS B TO OPEN THE SHOP"
            : $"REMAINING ENEMIES : {count:00}";
    }

    private void UpdateMoney(int money) =>
        moneyText.text = $"MONEY : {money:0000} $";

    private void UpdateDash(float currentDashPool)
    {
        for (int i = 0; i < dashSliders.Count; i++)
        {
            if (i < 3)
            {
                float value = (currentDashPool - (i * 4f)) / 4f;
                dashSliders[i].value = Mathf.Clamp01(value);
            }
            else
            {
                dashSliders[i].gameObject.SetActive(false);
            }
        }
    }
}
