using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaiterSpawner : MonoBehaviour
{
    public GameObject waiterPrefab;
    public Transform spawnPoint;

    public FactoryTab factoryTab;
    public OrderProcessor orderProcessor;

    public static event Action OnTabRefreshed;

    [Header("Configurações de Tempo")]
    public float initialDelay = 3f;
    public float delayBetweenWaiters = 5f;
    public int waitersPerDay = 3;
    private bool isCurrentOrderFinished = false;

    public List<WaiterData> dailyWaitersQueue = new List<WaiterData>();
    private Waiter currentWaiter;
    private OrderDebuff currentDebuffThisDay;

    public static event Action<Waiter> OnWaiterSpawned;

    private void OnEnable()
    {
        DayManager.OnDayStarted += HandleDayStarted;
        DayManager.OnDebuffChanged += HandleDebuffChanged;
        OrderProcessor.OnOrderFinished += HandleOrderFinished;
        PlayerDoubt.OnDoubtSubmitted += EvaluatePlayerDoubts;
    }

    private void OnDisable()
    {
        DayManager.OnDayStarted -= HandleDayStarted;
        DayManager.OnDebuffChanged -= HandleDebuffChanged;
        OrderProcessor.OnOrderFinished -= HandleOrderFinished;
        PlayerDoubt.OnDoubtSubmitted -= EvaluatePlayerDoubts;
    }

    private void HandleDayStarted(int currentDay)
    {
        GenerateDailyQueue();
        StartCoroutine(SpawnWaitersRoutine());
    }

    private void HandleDebuffChanged(OrderDebuff newDebuff)
    {
        currentDebuffThisDay = newDebuff;
    }

    private void HandleOrderFinished()
    {
        isCurrentOrderFinished = true;
    }

    private void GenerateDailyQueue()
    {
        dailyWaitersQueue.Clear();

        int liarsCount = Mathf.CeilToInt(waitersPerDay * 0.5f);
        int honestCount = waitersPerDay - liarsCount;

        TabRules rulesToApply = factoryTab.DefaultRules;
        if (currentDebuffThisDay != null)
        {
            rulesToApply = currentDebuffThisDay.ModifySpawnRules(rulesToApply);
        }

        for (int i = 0; i < liarsCount; i++)
        {
            Tab baseTab = null;
            Tab modifiedTab = null;
            int safetyCounter = 0;
            const int maxAttempts = 15;

            do
            {
                baseTab = factoryTab.CreateTab(rulesToApply);
                modifiedTab = orderProcessor.ProcessTab(baseTab);
                safetyCounter++;

            } while (modifiedTab == baseTab && safetyCounter < maxAttempts);


            WaiterData liarWaiter = new WaiterData();
            liarWaiter.originalTab = baseTab;
            liarWaiter.fakeTab = modifiedTab;

            liarWaiter.isLiar = (modifiedTab != baseTab);

            dailyWaitersQueue.Add(liarWaiter);
        }

        for (int i = 0; i < honestCount; i++)
        {
            Tab baseTab = factoryTab.CreateTab(rulesToApply);

            WaiterData honestWaiter = new WaiterData();
            honestWaiter.originalTab = baseTab;
            honestWaiter.fakeTab = baseTab;
            honestWaiter.isLiar = false;

            dailyWaitersQueue.Add(honestWaiter);
        }

        for (int i = dailyWaitersQueue.Count - 1; i > 0; i--)
        {
            int rnd = UnityEngine.Random.Range(0, i + 1);
            WaiterData temp = dailyWaitersQueue[i];
            dailyWaitersQueue[i] = dailyWaitersQueue[rnd];
            dailyWaitersQueue[rnd] = temp;
        }
    }

    private IEnumerator SpawnWaitersRoutine()
    {
        yield return new WaitForSeconds(initialDelay);

        for (int i = 0; i < waitersPerDay; i++)
        {
            isCurrentOrderFinished = false;


            CreatWaiter(i);

            yield return new WaitUntil(() => isCurrentOrderFinished);

            if (i < waitersPerDay - 1)
            {
                yield return new WaitForSeconds(delayBetweenWaiters);
            }
        }
    }

    public void CreatWaiter(int currentWaiterIndex)
    {
        if (currentWaiter != null) Destroy(currentWaiter.gameObject);
        if (currentWaiterIndex >= dailyWaitersQueue.Count) return;

        WaiterData currentData = dailyWaitersQueue[currentWaiterIndex];

        currentWaiter = Instantiate(waiterPrefab, spawnPoint.position, spawnPoint.rotation).GetComponent<Waiter>();

        currentWaiter.originalTab = currentData.originalTab;
        currentWaiter.fakeTab = currentData.fakeTab;
        currentWaiter.isLiar = currentData.isLiar;

        SpriteRenderer spriteRenderer = currentWaiter.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.color = UnityEngine.Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
        }

        OnWaiterSpawned?.Invoke(currentWaiter);
    }

    private void EvaluatePlayerDoubts(List<string> submittedDoubts)
    {
        if (currentWaiter == null) return;

        bool playerWasRight = false;

        foreach (string doubtId in submittedDoubts)
        {
            string[] parts = doubtId.Split('_');
            if (parts.Length < 2) continue;

            string doubtType = parts[0];
            if (!int.TryParse(parts[1], out int orderIndex)) continue;

            if (orderIndex >= currentWaiter.fakeTab.pedidos.Count ||
                orderIndex >= currentWaiter.originalTab.pedidos.Count) continue;

            Order fakeOrder = currentWaiter.fakeTab.pedidos[orderIndex];
            Order originalOrder = currentWaiter.originalTab.pedidos[orderIndex];

            if (doubtType == "qty" && fakeOrder.quantity != originalOrder.quantity)
            {
                fakeOrder.quantity = originalOrder.quantity;
                playerWasRight = true;
            }
            else if (doubtType == "ing" && AreIngredientsDifferent(fakeOrder.ingredientes, originalOrder.ingredientes))
            {
                fakeOrder.ingredientes = new List<Ingredient>(originalOrder.ingredientes);
                playerWasRight = true;
            }
        }

        if (playerWasRight)
        {
            OnTabRefreshed?.Invoke();
        }
    }

    private bool AreIngredientsDifferent(List<Ingredient> listA, List<Ingredient> listB)
    {
        if (listA.Count != listB.Count) return true;
        for (int i = 0; i < listA.Count; i++)
        {
            if (listA[i] != listB[i]) return true;
        }
        return false;
    }
}