using UnityEngine;
using Unity.Cinemachine;
using System;
using System.Collections;

public class ScreenShakeAction : MonoBehaviour
{
    [SerializeField] private ShakeSourceType shakeSource;
    [SerializeField] private ScreenShake screenShake;

    private void OnEnable()
    {
        StartCoroutine(WaitAndSubscribe());
    }

    private void OnDisable()
    {
        Unsubscribe();
    }

    private IEnumerator WaitAndSubscribe()
    {
        yield return new WaitUntil(() =>
            Player.Instance != null &&
            Player.Instance.GetActiveWeapon() != null &&
            Player.Instance.GetActiveWeapon().GetCurrentWeapon() != null
        );

        Subscribe();
    }

    private void Subscribe()
    {
        switch (shakeSource)
        {
            case ShakeSourceType.Recoil:
                Player.Instance.GetActiveWeapon().OnWeaponChanged += HandleWeaponChanged;
                var weapon = Player.Instance.GetActiveWeapon().GetCurrentWeapon();
                if (weapon != null) weapon.OnRecoil += HandleShake;
                break;

            case ShakeSourceType.Damaged:
                Player.Instance.OnPlayerDamaged += HandleShake;
                break;
        }
    }

    private void Unsubscribe()
    {
        switch (shakeSource)
        {
            case ShakeSourceType.Recoil:
                if (Player.Instance?.GetActiveWeapon() != null)
                {
                    Player.Instance.GetActiveWeapon().OnWeaponChanged -= HandleWeaponChanged;
                    var currentWeapon = Player.Instance.GetActiveWeapon().GetCurrentWeapon();
                    if (currentWeapon != null)
                        currentWeapon.OnRecoil -= HandleShake;
                }
                break;

            case ShakeSourceType.Damaged:
                if (Player.Instance?.GetHealth() != null)
                {
                    Player.Instance.OnPlayerDamaged -= HandleShake;
                }
                break;
        }
    }

    private void HandleWeaponChanged(Weapon newWeapon)
    {
        if (newWeapon != null)
        {
            newWeapon.OnRecoil += HandleShake;
        }
    }

    private void HandleShake(float strength)
    {
        screenShake.ShakeCamera(strength);
    }
}
