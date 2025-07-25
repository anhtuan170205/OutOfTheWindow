using UnityEngine;
using System.Collections;
using System;

public class Weapon : MonoBehaviour
{
    public event Action<Weapon> OnWeaponEquipped;
    public event Action<int> OnClipAmmoChanged;
    public event Action<int> OnAmmoChanged;
    public event Action<float> OnRecoil;
    public event Action OnShoot;
    public event Action OnReload;

    [Header("References")]
    [SerializeField] protected WeaponDetailsSO weaponDetails;
    [SerializeField] protected ParticleSystem muzzleFlash;
    [SerializeField] private GameObject WeaponModel;
    [SerializeField] protected Animator animator;
    [SerializeField] protected LayerMask ignoreLayer;
    [SerializeField] protected GameObject ammoPrefab;
    [SerializeField] protected Transform firePosition;
    protected int currentAmmo;
    protected int currentClipAmmo;
    protected float fireRateCooldownTimer = 0f;
    protected float firePrechargeTimer = 0f;
    protected bool isWeaponEquipped = false;
    protected bool isFiring = false;
    protected bool isReloading = false;
    protected int shootString = Animator.StringToHash("Shoot");
    protected int reloadString = Animator.StringToHash("Reload");
    protected int aimString = Animator.StringToHash("Aim");

    public virtual void Shoot()
    {
        if (IsWeaponReadyToShoot())
        {
            if (isReloading)
            {
                return;
            }

            if (!weaponDetails.HasInfiniteClipAmmo)
            {
                ChangeClipAmmo(currentClipAmmo - 1);
            }

            fireRateCooldownTimer = weaponDetails.FireRate;
            firePrechargeTimer = weaponDetails.PrechargeTime;
            muzzleFlash?.Play();
            animator.SetTrigger(shootString);
            FireAmmo();
            OnRecoil?.Invoke(weaponDetails.RecoilStrength);
            OnShoot?.Invoke();
        }
        else
        {
            if (currentClipAmmo <= 0)
            {
                Reload();
            }
        }
    }
    public virtual void Reload()
    {
        if (isReloading || !isWeaponEquipped)
        {
            return;
        }
        animator.SetTrigger(reloadString);
        OnReload?.Invoke();
        StartCoroutine(ReloadCoroutine());
    }

    public virtual void Equip()
    {
        isWeaponEquipped = true;
        WeaponModel.SetActive(true);

        if (currentAmmo == 0 && !weaponDetails.HasInfiniteAmmo)
        {
            int ammoToLoad = weaponDetails.MaxAmmo;
            ChangeAmmo(currentAmmo + ammoToLoad);
        }
        else
        {
            OnAmmoChanged?.Invoke(currentAmmo);
        }

        if (currentClipAmmo == 0 && !weaponDetails.HasInfiniteClipAmmo)
        {
            int ammoToLoad = Mathf.Min(weaponDetails.MaxClipAmmo, currentAmmo);
            ChangeClipAmmo(currentClipAmmo + ammoToLoad);
            ChangeAmmo(currentAmmo - ammoToLoad);
        }
        else
        {
            OnClipAmmoChanged?.Invoke(currentClipAmmo);
        }

        OnWeaponEquipped?.Invoke(this);
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
        if (isReloading)
        {
            return false;
        }

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

    protected IEnumerator ReloadCoroutine()
    {
        isReloading = true;
        yield return new WaitForSeconds(weaponDetails.ReloadTime);
        int ammoNeeded = weaponDetails.MaxClipAmmo - currentClipAmmo;

        if (weaponDetails.HasInfiniteAmmo)
        {
            int amountToMax = weaponDetails.MaxClipAmmo - currentClipAmmo;
            ChangeClipAmmo(currentClipAmmo + amountToMax);
        }
        else
        {
            int ammoToReload = Mathf.Min(ammoNeeded, currentAmmo);
            ChangeClipAmmo(currentClipAmmo + ammoToReload);
            ChangeAmmo(currentAmmo - ammoToReload);
        }
        isReloading = false;
    }

    private void FireAmmo()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;
        int layerMask = ~ignoreLayer.value;
        Vector3 targetPosition;

        if (Physics.Raycast(ray, out hit, weaponDetails.Range, layerMask))
        {
            Health health = hit.collider.GetComponent<Health>();
            if (health != null)
            {
                health.TakeDamage(weaponDetails.Damage);
            }

            targetPosition = hit.point;
        }
        else
        {
            targetPosition = Camera.main.transform.position + Camera.main.transform.forward * 50f;
        }

        GameObject ammoInstance = Instantiate(ammoPrefab, firePosition.position, Quaternion.identity);
        Projectile projectile = ammoInstance.GetComponent<Projectile>();
        if (projectile != null)
        {
            Vector3 direction = (targetPosition - firePosition.position).normalized + Vector3.down * 0.005f;
            projectile.Launch(direction);
        }
    }


    private void ChangeClipAmmo(int amount)
    {
        currentClipAmmo = amount;
        OnClipAmmoChanged?.Invoke(currentClipAmmo);
    }

    private void ChangeAmmo(int amount)
    {
        currentAmmo = amount;
        OnAmmoChanged?.Invoke(currentAmmo);
    }

    public void AddAmmo(int amount)
    {
        if (weaponDetails.HasInfiniteAmmo)
        {
            return;
        }
        ChangeAmmo(currentAmmo + amount);
    }

    public int GetCurrentAmmo()
    {
        return currentAmmo;
    }

    public int GetCurrentClipAmmo()
    {
        return currentClipAmmo;
    }

    public void Aim(bool isAiming)
    {
        animator.SetBool(aimString, isAiming);
    }
}
