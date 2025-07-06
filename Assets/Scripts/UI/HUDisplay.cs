using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

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
    [SerializeField] private TextMeshProUGUI dayTimerText;
    [SerializeField] private TextMeshProUGUI enemyCountText;
    [SerializeField] private TextMeshProUGUI moneyText;

    private float maxShield = 100f;
    private float maxHealth = 100f;
    private Weapon currentBoundWeapon;

    private void OnEnable()
    {
        StartCoroutine(WaitForPlayerAndBind());
    }

    private IEnumerator WaitForPlayerAndBind()
    {
        yield return new WaitUntil(() =>
            Player.Instance != null &&
            Player.Instance.GetActiveWeapon() != null &&
            Player.Instance.GetActiveWeapon().GetCurrentWeapon() != null &&
            Player.Instance.GetHealth() != null &&
            Player.Instance.GetShield() != null &&
            Player.Instance.GetMoneyWallet() != null &&
            TurnManager.Instance != null &&
            DayNightManager.Instance != null
        );

        Player.Instance.GetActiveWeapon().OnWeaponChanged += HandleWeaponChanged;
        BindWeaponEvents(Player.Instance.GetActiveWeapon().GetCurrentWeapon());
        Player.Instance.GetHealth().OnPlayerHealthChanged += UpdateHealth;
        Player.Instance.GetHealth().OnPlayerMaxHealthChanged += (max) => { maxHealth = max; };
        TurnManager.Instance.OnTurnChanged += UpdateDay;
        TurnManager.Instance.OnDayTimerChanged += UpdateDayTimer;
        DayNightManager.Instance.OnStateChanged += UpdateTime;
        EnemySpawner.Instance.OnEnemyCountChanged += UpdateEnemyCount;
        Player.Instance.GetShield().OnShieldChanged += UpdateShield;
        Player.Instance.GetShield().OnMaxShieldChanged += (max) => { maxShield = max; };
        Player.Instance.GetMoneyWallet().OnMoneyChanged += UpdateMoney;
    }

    private void OnDisable()
    {
        if (currentBoundWeapon != null)
        {
            currentBoundWeapon.OnClipAmmoChanged -= UpdateAmmoClip;
            currentBoundWeapon.OnAmmoChanged -= UpdateAmmo;
        }

        if (Player.Instance != null && Player.Instance.GetActiveWeapon() != null)
        {
            Player.Instance.GetActiveWeapon().OnWeaponChanged -= HandleWeaponChanged;
        }

        if (Player.Instance != null)
        {
            Player.Instance.GetHealth().OnPlayerHealthChanged -= UpdateHealth;
            Player.Instance.GetHealth().OnPlayerMaxHealthChanged -= (max) => { maxHealth = max; };
            Player.Instance.GetShield().OnShieldChanged -= UpdateShield;
            Player.Instance.GetShield().OnMaxShieldChanged -= (max) => { maxShield = max; };
            Player.Instance.GetMoneyWallet().OnMoneyChanged -= UpdateMoney;
        }

        if (TurnManager.Instance != null)
        {
            TurnManager.Instance.OnTurnChanged -= UpdateDay;
            TurnManager.Instance.OnDayTimerChanged -= UpdateDayTimer;
        }

        if (DayNightManager.Instance != null)
        {
            DayNightManager.Instance.OnStateChanged -= UpdateTime;
        }

        if (EnemySpawner.Instance != null)
        {
            EnemySpawner.Instance.OnEnemyCountChanged -= UpdateEnemyCount;
        }
    }

    private void Start()
    {
        dayTimerText.gameObject.SetActive(true);
        UpdateTime(DayNightManager.Instance.CurrentState);
        UpdateEnemyCount(0);
        UpdateMoney(0);
    }

    private void HandleWeaponChanged(Weapon newWeapon)
    {
        BindWeaponEvents(newWeapon);
    }

    private void BindWeaponEvents(Weapon weapon)
    {
        if (currentBoundWeapon != null)
        {
            currentBoundWeapon.OnClipAmmoChanged -= UpdateAmmoClip;
            currentBoundWeapon.OnAmmoChanged -= UpdateAmmo;
        }

        currentBoundWeapon = weapon;
        currentBoundWeapon.OnClipAmmoChanged += UpdateAmmoClip;
        currentBoundWeapon.OnAmmoChanged += UpdateAmmo;
        UpdateAmmoClip(currentBoundWeapon.GetCurrentClipAmmo());
        UpdateAmmo(currentBoundWeapon.GetCurrentAmmo());
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
        dayText.text = day.ToString();
    }

    private void UpdateDayTimer(int seconds)
    {
        dayTimerText.text = "DAY DURATION : " + seconds.ToString("00") + "s";
    }

    private void UpdateTime(DayNightState state)
    {
        timeText.text = state == DayNightState.Day ? "DAY" : "NIGHT";
        dayTimerText.gameObject.SetActive(state == DayNightState.Day);
    }

    private void UpdateEnemyCount(int count)
    {
        if (count == 0)
        {
            enemyCountText.text = "PRESS B TO OPEN THE SHOP";
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
