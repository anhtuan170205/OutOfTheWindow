using UnityEngine;
using Unity.Cinemachine;

public class ScreenShakeAction : MonoBehaviour
{
    private void Start()
    {
        Weapon.OnRecoil += HandleRecoil;
    }

    private void HandleRecoil(float recoilStrength)
    {
        ScreenShake.Instance.ShakeCamera(recoilStrength);
    }
}
