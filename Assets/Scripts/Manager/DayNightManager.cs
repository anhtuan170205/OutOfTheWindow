using UnityEngine;
using System;

public class DayNightManager : SingletonMonoBehaviour<DayNightManager>
{
    public static event Action<DayNightState> OnStateChanged;
    public DayNightState CurrentState { get; private set; } = DayNightState.Day;
    public void SetState(DayNightState newState)
    {
        if (CurrentState != newState)
        {
            CurrentState = newState;
            OnStateChanged?.Invoke(CurrentState);
        }
    }
}
