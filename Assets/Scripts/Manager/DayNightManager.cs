using UnityEngine;
using System;

public class DayNightManager : MonoBehaviour
{
    public event Action<DayNightState> OnStateChanged;
    private DayNightState currentState;

    public DayNightState CurrentState => currentState;

    public void SetState(DayNightState state)
    {
        currentState = state;
        OnStateChanged?.Invoke(currentState);
    }
}
