using System;
using UnityEngine;

public class DayManager : MonoBehaviour
{
    [Header("Configurações do Dia")]
    [SerializeField] private int totalDays = 5;
    public float CurrentTimeInDay { get; private set; }
    public float CustomerSatisfaction { get; private set; } = 100f;
    public bool IsDayActive { get; private set; }

    public static event Action<OrderDebuff> OnDebuffChanged;
    public static event Action<int> OnDayStarted;
    public static event Action OnDayEnded;
    public static event Action<float> OnSatisfactionChanged;
    public static event Action OnGameOver;

    public GameData gameData;


    private void OnEnable()
    {
        WaiterSpawner.OnQueueEnded += EndCurrentDay;
    }

    private void OnDisable()
    {
        WaiterSpawner.OnQueueEnded -= EndCurrentDay;
    }

    void Start()
    {
        StartNewDay();
    }
    public void DisableUI(GameObject ui)
    {
        ui.SetActive(false);
        StartNewDay();
    }
    public void StartNewDay()
    {
        if (gameData.currenteDay > totalDays) return;
        gameData.point = 0;
        CurrentTimeInDay = 0f;
        IsDayActive = true;

        OrderDebuff activeDebuff = ChooseDebuffForCurrentDay();


        OnDebuffChanged?.Invoke(activeDebuff);
        OnDayStarted?.Invoke(gameData.currenteDay);
    }

    private OrderDebuff ChooseDebuffForCurrentDay()
    {
        int random = UnityEngine.Random.Range(0, 3);
        switch (random)
        {
            case 0: return new ChangeQuantityModifier();
            case 1: return new SwapDrinkFoodModifier();
            default: return new CategorySwapModifier();
        }
    }

    public void EndCurrentDay()
    {
        if (!IsDayActive) return;

        IsDayActive = false;
        OnDebuffChanged?.Invoke(null);

        OnDayEnded?.Invoke();
        gameData.currenteDay++;
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