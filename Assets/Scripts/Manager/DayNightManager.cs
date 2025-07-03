using UnityEngine;
using System;

public class DayNightManager : SingletonMonoBehaviour<DayNightManager>
{
    public event Action<DayNightState> OnStateChanged;
    private DayNightState currentState;
    public DayNightState CurrentState
    {
        get => currentState;
        private set
        {
            currentState = value;
            OnStateChanged?.Invoke(currentState);
        }
    }

    public void SetState(DayNightState state)
    {
        CurrentState = state;
    }
}
