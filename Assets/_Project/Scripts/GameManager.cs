using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Configurações de Spawn")]
    public GameObject tabPrefab;
    public Transform spawnPoint;
    int count = 0;

    [SerializeField] private FactoryTab factory;
    [SerializeField] private OrderProcessor processor;
    [SerializeField] private TabView view;

    private SwapDrinkFoodModifier swapModifier = new SwapDrinkFoodModifier();

    public void Start()
    {
        processor.AddModifier(swapModifier);
        for (int i = 0; i < 4; i++) GenerateNewOrder();
    }


    public void GenerateNewOrder()
    {
        Tab originalTab = factory.CreateTab();
        float random = Random.Range(0f, 1f);
        if (random > 0.5f)
        {
            Tab fakeTab = processor.ProcessTab(originalTab);
            SpawnTab(fakeTab, originalTab);
        }
        else
        {
            SpawnTab(originalTab, originalTab);
        }
    }

    public void SpawnTab(Tab fakeTab, Tab originalTab)
    {
        count++;
        GameObject tabInstance = Instantiate(tabPrefab, spawnPoint.position, Quaternion.identity, spawnPoint);
        TabInteract interact = tabInstance.GetComponent<TabInteract>();
        interact.tabData = fakeTab;
        interact.originalData = originalTab;
        interact.count = count;
    }
}