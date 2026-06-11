using System;
using UnityEngine;

public class DayManager : MonoBehaviour
{
    [Header("Configurações do Dia")]
    [SerializeField] private int totalDays = 5;
    [SerializeField] private float dayDurationInSeconds = 120f;

    public int CurrentDay { get; private set; } = 1;
    public float CurrentTimeInDay { get; private set; }
    public float CustomerSatisfaction { get; private set; } = 100f;
    public bool IsDayActive { get; private set; }
    public static event Action<int> OnDayStarted;
    public static event Action OnDayEnded;
    public static event Action<float> OnSatisfactionChanged;
    public static event Action OnGameOver;

    void Start()
    {
        StartNewDay();
    }
    public void StartNewDay()
    {
        if (CurrentDay > totalDays) return;
        CurrentTimeInDay = 0f;
        IsDayActive = true;

        Debug.Log($"Dia {CurrentDay} começou!");
        OnDayStarted?.Invoke(CurrentDay);
    }
    public void EndCurrentDay()
    {
        IsDayActive = false;
        Debug.Log($"Dia {CurrentDay} terminou!");

        OnDayEnded?.Invoke();
        CurrentDay++;
    }
    public void ModifySatisfaction(float amount) // Talvez alterar isso
    {
        CustomerSatisfaction = Mathf.Clamp(CustomerSatisfaction + amount, 0f, 100f);
        OnSatisfactionChanged?.Invoke(CustomerSatisfaction);

        if (CustomerSatisfaction <= 0f)
        {
            IsDayActive = false;
            OnGameOver?.Invoke();
        }
    }
}
