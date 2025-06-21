using UnityEngine;
using System;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] protected WeaponDetailsSO weaponDetails;
    [SerializeField] protected ParticleSystem muzzleFlash;
    [SerializeField] private GameObject WeaponModel;
    [SerializeField] protected Animator animator;
    protected int currentAmmo;
    protected int currentClipAmmo;
    protected float fireRateCooldownTimer = 0f;
    protected float firePrechargeTimer = 0f;
    protected bool isWeaponEquipped = false;
    protected bool isFiring = false;
    protected int shootString = Animator.StringToHash("Shoot");

    public virtual void Shoot()
    {
        if (IsWeaponReadyToShoot())
        {
            if (!weaponDetails.HasInfiniteClipAmmo)
            {
                currentClipAmmo--;
            }

            if (!weaponDetails.HasInfiniteAmmo)
            {
                currentAmmo--;
            }

            fireRateCooldownTimer = weaponDetails.FireRate;
            firePrechargeTimer = weaponDetails.PrechargeTime;
            muzzleFlash?.Play();
            animator.SetTrigger(shootString);
        }
        else
        {
            if (currentClipAmmo <= 0)
            {
                Reload();
            }
        }
        Debug.Log($"Shooting {weaponDetails.name}. Current Ammo: {currentAmmo}, Current Clip Ammo: {currentClipAmmo}");
    }
    public virtual void Reload()
    {
        int ammoNeeded = weaponDetails.MaxClipAmmo - currentClipAmmo;

        if (weaponDetails.HasInfiniteAmmo)
        {
            currentClipAmmo = weaponDetails.MaxClipAmmo;
        }
        else
        {
            int ammoToReload = Mathf.Min(ammoNeeded, currentAmmo);
            currentClipAmmo += ammoToReload;
            currentAmmo -= ammoToReload;
        }
    }

    public virtual void Equip()
    {
        isWeaponEquipped = true;
        WeaponModel.SetActive(true);

        if (currentAmmo == 0 && !weaponDetails.HasInfiniteAmmo)
        {
            currentAmmo = weaponDetails.MaxAmmo;
        }

        if (currentClipAmmo == 0 && !weaponDetails.HasInfiniteClipAmmo)
        {
            int ammoToLoad = Mathf.Min(weaponDetails.MaxClipAmmo, currentAmmo);
            currentClipAmmo = ammoToLoad;
            currentAmmo -= ammoToLoad;
        }
    }

    public virtual void Unequip()
    {
        isWeaponEquipped = false;
        WeaponModel.SetActive(false);
    }

    protected virtual void Update()
    {
        if (!isWeaponEquipped)
        {
            return;
        }
        fireRateCooldownTimer -= Time.deltaTime;
        firePrechargeTimer -= Time.deltaTime;
    }

    public bool IsWeaponReadyToShoot()
    {
        if (weaponDetails == null) return false;

        if (!weaponDetails.HasInfiniteClipAmmo && currentClipAmmo <= 0)
        {
            return false;
        }

        if (fireRateCooldownTimer > 0f || firePrechargeTimer > 0f)
        {
            return false;
        }

        return true;
    }

}
