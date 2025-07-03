using UnityEngine;
using Unity.Cinemachine;
using System.Collections;

public class ScreenShakeAction : MonoBehaviour
{
    private void OnEnable()
    {
        StartCoroutine(WaitForWeaponAndSubscribe());
    }

    private void OnDisable()
    {
        Player.Instance.GetActiveWeapon().GetCurrentWeapon().OnRecoil -= HandleRecoil;
    }

    private IEnumerator WaitForWeaponAndSubscribe()
    {
        yield return new WaitUntil(() =>
            Player.Instance != null &&
            Player.Instance.GetActiveWeapon() != null &&
            Player.Instance.GetActiveWeapon().GetCurrentWeapon() != null
        );

        Player.Instance.GetActiveWeapon().GetCurrentWeapon().OnRecoil += HandleRecoil;
    }


    private void HandleRecoil(float recoilStrength)
    {
        ScreenShake.Instance.ShakeCamera(recoilStrength);
    }
}
