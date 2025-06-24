using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI ammoClipText;
    [SerializeField] private TextMeshProUGUI ammoText;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private TextMeshProUGUI healthText;

    private void OnEnable()
    {
        Weapon.OnClipAmmoChanged += UpdateAmmoClip;
        Weapon.OnAmmoChanged += UpdateAmmo;
        Health.OnPlayerHealthChanged += UpdateHealth;
    }

    private void OnDisable()
    {
        Weapon.OnClipAmmoChanged -= UpdateAmmoClip;
        Weapon.OnAmmoChanged -= UpdateAmmo;
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
}
