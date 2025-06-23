using UnityEngine;

[CreateAssetMenu(fileName = "WeaponDetailsSO", menuName = "Scriptable Objects/WeaponDetailsSO")]
public class WeaponDetailsSO : ScriptableObject
{
    public bool HasInfiniteAmmo;
    public bool HasInfiniteClipAmmo;
    public int MaxAmmo;
    public int MaxClipAmmo;
    public float ReloadTime;
    public float FireRate;
    public float PrechargeTime;
    public int Damage;
    public float Range;
    public Vector2 RecoilDir;
    public float RecoilStrength;
    public float RecoilReturnSpeed;
}
