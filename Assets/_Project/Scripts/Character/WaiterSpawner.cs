using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaiterSpawner : MonoBehaviour
{
    public GameObject waiterPrefab;
    public Transform spawnPoint;

    public FactoryTab factoryTab;
    public OrderProcessor orderProcessor;

    [Header("Configurações de Tempo")]
    public float initialDelay = 3f;
    public float delayBetweenWaiters = 5f;
    public int waitersPerDay = 3;
    private bool isCurrentOrderFinished = false;

    private Waiter currentWaiter;

    private void OnEnable()
    {
        DayManager.OnDayStarted += HandleDayStarted;
        OrderProcessor.OnOrderFinished += HandleOrderFinished;
    }

    private void OnDisable()
    {
        DayManager.OnDayStarted -= HandleDayStarted;
        OrderProcessor.OnOrderFinished -= HandleOrderFinished;
    }

    private void HandleDayStarted(int currentDay)
    {
        StartCoroutine(SpawnWaitersRoutine());
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
        if (currentWaiter != null)
        {
            Destroy(currentWaiter.gameObject);
        }

        Tab newTab = factoryTab.CreateTab();
        currentWaiter = Instantiate(waiterPrefab, spawnPoint.position, spawnPoint.rotation).GetComponent<Waiter>();
        currentWaiter.originalTab = newTab;

        SpriteRenderer spriteRenderer = currentWaiter.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
        }
    }
}