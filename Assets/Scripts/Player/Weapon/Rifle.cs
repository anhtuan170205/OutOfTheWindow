using UnityEngine;

public class Rifle : Weapon
{
    public override void Shoot()
    {
        Debug.Log("Rifle shot fired!");
    }

    public override void Reload()
    {
        Debug.Log("Rifle reloaded!");
    }

    public override void Equip()
    {
        base.Equip();
        Debug.Log("Rifle equipped!");
    }

    public override void Unequip()
    {
        base.Unequip();
        Debug.Log("Rifle unequipped!");
    }
}
