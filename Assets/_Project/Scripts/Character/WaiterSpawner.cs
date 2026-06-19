using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class WaiterSpawner : MonoBehaviour
{
    [Header("Configurações dos Garçons")]
    public List<GameObject> waiterPrefabs = new List<GameObject>();
    public Transform spawnPoint;

    [Header("Sistemas de Validação")]
    public IngredientContainer squeezerContainer;
    public IngredientContainer shakerContainer;
    public IngredientContainer foodContainer;

    [Header("Pontuação")]
    public int scorePerCorrectItem = 10;
    public GameData gameData;

    public FactoryTab factoryTab;
    public OrderProcessor orderProcessor;

    public static event Action OnTabRefreshed;

    // --- NOVO EVENTO: A FILA DO DIA ACABOU ---
    public static event Action OnQueueEnded;

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
        if (currentWaiter == null || currentWaiter.originalTab == null)
        {
            isCurrentOrderFinished = true;
            return;
        }

        Debug.Log("--- Iniciando checagem dos itens entregues ---");

        List<Ingredient> preparedIngredients = new List<Ingredient>();

        if (squeezerContainer != null) preparedIngredients.AddRange(squeezerContainer.ingredients);
        if (shakerContainer != null) preparedIngredients.AddRange(shakerContainer.ingredients);
        if (foodContainer != null) preparedIngredients.AddRange(foodContainer.ingredients);

        List<Ingredient> requiredIngredients = new List<Ingredient>();
        foreach (var pedido in currentWaiter.originalTab.pedidos)
        {
            for (int i = 0; i < pedido.quantity; i++)
            {
                requiredIngredients.AddRange(pedido.ingredientes);
            }
        }

        int correctMatches = 0;
        List<Ingredient> tempPrepared = new List<Ingredient>(preparedIngredients);

        foreach (var reqIng in requiredIngredients)
        {
            if (tempPrepared.Contains(reqIng))
            {
                correctMatches++;
                tempPrepared.Remove(reqIng);
            }
        }

        int pointsGained = correctMatches * scorePerCorrectItem;
        gameData.point += pointsGained;
        Debug.Log($"Entrega concluída! Acertos: {correctMatches}/{requiredIngredients.Count}. Pontos Ganhos: {pointsGained}. Total: {gameData}");

        if (squeezerContainer != null) squeezerContainer.ClearIngredients();
        if (shakerContainer != null) shakerContainer.ClearIngredients();
        if (foodContainer != null) foodContainer.ClearIngredients();

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

            CreateWaiter(i);

            yield return new WaitUntil(() => isCurrentOrderFinished);

            if (i < waitersPerDay - 1)
            {
                yield return new WaitForSeconds(delayBetweenWaiters);
            }
        }

        Debug.Log("Fila de garçons esgotada. Disparando OnQueueEnded.");
        OnQueueEnded?.Invoke();
    }

    public void CreateWaiter(int currentWaiterIndex)
    {
        if (currentWaiterIndex >= dailyWaitersQueue.Count) return;

        if (waiterPrefabs == null || waiterPrefabs.Count == 0)
        {
            Debug.LogError("Nenhum Waiter Prefab associado na lista do WaiterSpawner!");
            return;
        }

        if (currentWaiter != null)
        {
            currentWaiter.AnimateExitAndDestroy();
        }

        WaiterData currentData = dailyWaitersQueue[currentWaiterIndex];

        int randomPrefabIndex = UnityEngine.Random.Range(0, waiterPrefabs.Count);
        GameObject chosenPrefab = waiterPrefabs[randomPrefabIndex];

        GameObject waiterObj = Instantiate(chosenPrefab, spawnPoint.position, spawnPoint.rotation);
        currentWaiter = waiterObj.GetComponent<Waiter>();

        currentWaiter.originalTab = currentData.originalTab;
        currentWaiter.fakeTab = currentData.fakeTab;
        currentWaiter.isLiar = currentData.isLiar;

        Vector3 originalScale = waiterObj.transform.localScale;
        waiterObj.transform.localScale = Vector3.zero;

        Sequence entrySequence = DOTween.Sequence();
        entrySequence.Append(waiterObj.transform.DOScale(originalScale, 0.3f).SetEase(Ease.OutBack));
        entrySequence.Append(waiterObj.transform.DOShakePosition(0.2f, 0.1f, 1));

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