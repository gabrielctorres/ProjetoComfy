using System;
using UnityEngine;

public class DayManager : MonoBehaviour
{
    [Header("Configurações do Dia")]
    [SerializeField] private int totalDays = 5;
    public int CurrentDay { get; private set; } = 1;
    public float CurrentTimeInDay { get; private set; }
    public float CustomerSatisfaction { get; private set; } = 100f;
    public bool IsDayActive { get; private set; }

    public static event Action<OrderDebuff> OnDebuffChanged;

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

        OrderDebuff activeDebuff = ChooseDebuffForCurrentDay();

        Debug.Log($"Dia {CurrentDay} começou, Debuff Ativo: {(activeDebuff != null ? activeDebuff.Name : "Nenhum")}");

        OnDebuffChanged?.Invoke(activeDebuff);
        OnDayStarted?.Invoke(CurrentDay);
    }

    private OrderDebuff ChooseDebuffForCurrentDay()
    {
        int random = 1; // mudar isso dps
        switch (random)
        {
            case 0: return new ChangeQuantityModifier();
            case 1: return new SwapDrinkFoodModifier();
            default: return new CategorySwapModifier();
        }
    }

    public void EndCurrentDay()
    {
        IsDayActive = false;
        OnDebuffChanged?.Invoke(null);

        Debug.Log($"Dia {CurrentDay} terminou");
        OnDayEnded?.Invoke();
        CurrentDay++;
    }

    public void ModifySatisfaction(float amount)
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