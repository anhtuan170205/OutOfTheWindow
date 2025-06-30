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
        currentWeaponIndex = (currentWeaponIndex + 1) % weaponList.Count;
        currentWeapon = weaponList[currentWeaponIndex];
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

    }

    public void AddAmmo(int amount)
    {
        currentWeapon.AddAmmo(amount);
    }
    
}
