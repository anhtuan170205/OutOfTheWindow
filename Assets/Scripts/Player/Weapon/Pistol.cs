using UnityEngine;

public class Pistol : Weapon
{
    public override void Shoot()
    {
        Debug.Log("Pistol shot fired!");
    }

    public override void Reload()
    {
        Debug.Log("Pistol reloaded!");
    }

    public override void Equip()
    {
        base.Equip();
        Debug.Log("Pistol equipped!");
    }
    
    public override void Unequip()
    {
        base.Unequip();
        Debug.Log("Pistol unequipped!");
    }
}
