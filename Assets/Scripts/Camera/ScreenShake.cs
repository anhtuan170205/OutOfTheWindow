using UnityEngine;
using Unity.Cinemachine;

public class ScreenShake : SingletonMonoBehaviour<ScreenShake>
{
    private CinemachineImpulseSource impulseSource;
    private void Start()
    {
        impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    public void ShakeCamera(float intensity = 1f)
    {
        impulseSource.GenerateImpulse(intensity);
    }
}
