using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public string WeaponName;
    public int WeaponDamage;
    public int WeaponMaxAmmo;
    public int WeaponCurrentAmmo;

    public abstract void Shoot();
    public abstract void Reload();
    public virtual void Equip()
    {
        gameObject.SetActive(true);
    }

    public virtual void Unequip()
    {
        gameObject.SetActive(false);
    }


}
