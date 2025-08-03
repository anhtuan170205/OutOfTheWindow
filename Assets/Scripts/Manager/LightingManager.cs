using UnityEngine;
using UnityEngine.Rendering;

public class LightingManager : MonoBehaviour
{
    [SerializeField] private Material daySkybox;
    [SerializeField] private Material nightSkybox;

    public void UpdateLighting(DayNightState state)
    {
        switch (state)
        {
            case DayNightState.Day:
                RenderSettings.skybox = daySkybox;
                break;
            case DayNightState.Night:
                RenderSettings.skybox = nightSkybox;
                break;
        }
    }
}
