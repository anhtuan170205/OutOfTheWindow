using UnityEngine;
using System;
using UnityEngine.Rendering;

public class LightingManager : SingletonMonoBehaviour<LightingManager>
{
    [SerializeField] private Material daySkybox;
    [SerializeField] private Material nightSkybox;

    private void OnEnable()
    {
        DayNightManager.Instance.OnStateChanged += UpdateLighting;
    }

    private void OnDisable()
    {
        DayNightManager.Instance.OnStateChanged -= UpdateLighting;
    }

    private void UpdateLighting(DayNightState state)
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
