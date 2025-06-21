using UnityEngine;

[CreateAssetMenu(fileName = "WeaponDetailsSO", menuName = "Scriptable Objects/WeaponDetailsSO")]
public class WeaponDetailsSO : ScriptableObject
{
    public bool HasInfiniteAmmo;
    public bool HasInfiniteClipAmmo;
    public bool IsAutomatic;
    public int MaxAmmo;
    public int MaxClipAmmo;
    public float ReloadTime;
    public float FireRate;
    public float PrechargeTime;
    public float Damage;
    public float Range;
}
