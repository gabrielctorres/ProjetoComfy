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

    private IEnumerator SpawnWaitersRoutine()
    {
        yield return new WaitForSeconds(initialDelay);

        for (int i = 0; i < waitersPerDay; i++)
        {
            isCurrentOrderFinished = false;

            CreatWaiter();

            yield return new WaitUntil(() => isCurrentOrderFinished);

            if (i < waitersPerDay - 1)
            {
                yield return new WaitForSeconds(delayBetweenWaiters);
            }
        }
    }

    public void CreatWaiter()
    {
        if (currentWaiter != null) Destroy(currentWaiter.gameObject);

        SpawnRules rulesToApply = factoryTab.DefaultRules;

        if (currentDebuffThisDay != null)
        {
            rulesToApply = currentDebuffThisDay.ModifySpawnRules(rulesToApply);
        }

        Tab newTab = factoryTab.CreateTab(rulesToApply);

        currentWaiter = Instantiate(waiterPrefab, spawnPoint.position, spawnPoint.rotation).GetComponent<Waiter>();
        currentWaiter.originalTab = newTab;
        currentWaiter.fakeTab = orderProcessor.ProcessTab(newTab);


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