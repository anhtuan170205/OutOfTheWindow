using UnityEngine;
using System.Collections.Generic;

public class ActiveWeapon : MonoBehaviour
{
    [SerializeField] private InputReader inputReader;
    [SerializeField] private List<Weapon> weaponList;
    [SerializeField] private int currentWeaponIndex = 0;
    private bool isFiring = false;
    private Weapon currentWeapon;

    private void Start()
    {
        currentWeapon = weaponList[currentWeaponIndex];
        for (int i = 0; i < weaponList.Count; i++)
        {
            weaponList[i].Unequip();
        }
        currentWeapon.Equip();
    }

    private void OnEnable()
    {
        inputReader.ShootEvent += HandleShoot;
        inputReader.ReloadEvent += HandleReload;
        inputReader.SwapEvent += HandleSwap;
    }

    private void OnDisable()
    {
        inputReader.ShootEvent -= HandleShoot;
        inputReader.ReloadEvent -= HandleReload;
        inputReader.SwapEvent -= HandleSwap;
    }

    private void HandleShoot(bool isShooting)
    {
        isFiring = isShooting;
    }

    private void HandleReload(bool isReload)
    {
        currentWeapon.Reload();
    }

    private void HandleSwap(bool isSwap)
    {
        currentWeapon.Unequip();
        int startIndex = currentWeaponIndex;
        do
        {
            currentWeaponIndex = (currentWeaponIndex + 1) % weaponList.Count;
            Weapon nextWeapon = weaponList[currentWeaponIndex];
            if (nextWeapon is Rifle rifle && !rifle.IsUnlocked)
            {
                continue;
            }
            currentWeapon = nextWeapon;
            currentWeapon.Equip();
            return;
        } while (currentWeaponIndex != startIndex);
        currentWeapon.Equip();
    }
    private void Update()
    {
        if (isFiring)
        {
            currentWeapon.Shoot();
        }
    }

    public void UnlockRifle()
    {
        foreach (Weapon weapon in weaponList)
        {
            if (weapon is Rifle rifle && !rifle.IsUnlocked)
            {
                rifle.Unlock();
                Debug.Log("Rifle unlocked!");
                return;
            }
        }
    }

    public void AddAmmo(int amount)
    {
        currentWeapon.AddAmmo(amount);
    }
    
}
