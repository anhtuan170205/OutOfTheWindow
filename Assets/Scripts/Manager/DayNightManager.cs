using UnityEngine;
using System;

public class DayNightManager : SingletonMonoBehaviour<DayNightManager>
{
    public DayNightState CurrentState { get; private set; } = DayNightState.Day;
    public static event Action<DayNightState> OnStateChanged;

    public void SetState(DayNightState newState)
    {
        if (CurrentState != newState)
        {
            CurrentState = newState;
            OnStateChanged?.Invoke(CurrentState);
        }
    }
}
