using UnityEngine;

public class Rifle : Weapon
{
    [SerializeField] private bool isUnlocked = false;
    public bool IsUnlocked => isUnlocked;

    public void Unlock()
    {
        isUnlocked = true;
    }
}
