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

        Player.Instance.GetActiveWeapon().GetCurrentWeapon().OnClipAmmoChanged += UpdateAmmoClip;
        Player.Instance.GetActiveWeapon().GetCurrentWeapon().OnAmmoChanged += UpdateAmmo;
        Player.Instance.GetHealth().OnPlayerHealthChanged += UpdateHealth;
        Player.Instance.GetHealth().OnPlayerMaxHealthChanged += (max) => { maxHealth = max; };
        TurnManager.Instance.OnTurnChanged += UpdateDay;
        TurnManager.Instance.OnDayTimerChanged += UpdateDayTimer;
        DayNightManager.Instance.OnStateChanged += UpdateTime;
        EnemySpawner.OnEnemyCountChanged += UpdateEnemyCount;
        Player.Instance.GetShield().OnShieldChanged += UpdateShield;
        Player.Instance.GetShield().OnMaxShieldChanged += (max) => { maxShield = max; };
        Player.Instance.GetMoneyWallet().OnMoneyChanged += UpdateMoney;
    }


    private void OnDisable()
    {
        Player.Instance.GetActiveWeapon().GetCurrentWeapon().OnClipAmmoChanged -= UpdateAmmoClip;
        Player.Instance.GetActiveWeapon().GetCurrentWeapon().OnAmmoChanged -= UpdateAmmo;
        Player.Instance.GetHealth().OnPlayerHealthChanged -= UpdateHealth;
        Player.Instance.GetHealth().OnPlayerMaxHealthChanged -= (maxHealth) => { this.maxHealth = maxHealth; };
        TurnManager.Instance.OnTurnChanged -= UpdateDay;
        TurnManager.Instance.OnDayTimerChanged -= UpdateDayTimer;
        DayNightManager.Instance.OnStateChanged -= UpdateTime;
        EnemySpawner.OnEnemyCountChanged -= UpdateEnemyCount;
        Player.Instance.GetShield().OnShieldChanged -= UpdateShield;
        Player.Instance.GetShield().OnMaxShieldChanged -= (maxShield) => { this.maxShield = maxShield; };
        Player.Instance.GetMoneyWallet().OnMoneyChanged -= UpdateMoney;
    }

    private void Start()
    {
        dayTimerText.gameObject.SetActive(true);
        UpdateTime(DayNightManager.Instance.CurrentState);
        UpdateEnemyCount(0);
        UpdateMoney(0);
        UpdateAmmo(0);
        UpdateAmmoClip(10);
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
        if (DayNightManager.Instance.CurrentState == DayNightState.Day)
        {
            dayTimerText.gameObject.SetActive(true);
            dayTimerText.color = Color.white;
            dayTimerText.text = "DAY DURATION : " + seconds.ToString("00") + "s";
        }
        else
        {
            dayTimerText.gameObject.SetActive(false);
        }
    }

    private void UpdateTime(DayNightState state)
    {
        timeText.text = state == DayNightState.Day ? "DAY" : "NIGHT";
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
