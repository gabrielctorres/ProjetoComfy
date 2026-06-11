using System;
using UnityEngine;

public class Waiter : MonoBehaviour
{
    public Tab originalTab;
    public Tab fakeTab;
    public bool isLiar;

    GameObject prefabTab;
    Transform spawnPoint;

    private GameObject currentTabInstance;

    void Start()
    {
        prefabTab = GameManager.Instance.tabPrefab;
        spawnPoint = GameManager.Instance.tabPoint;
        SpawnTab();
    }

    public void SpawnTab()
    {
        currentTabInstance = Instantiate(prefabTab, spawnPoint.position, Quaternion.identity, spawnPoint);

        TabInteract interact = currentTabInstance.GetComponent<TabInteract>();
        if (interact != null)
        {
            interact.tabData = originalTab;
        }
    }
    public void DestroyTab()
    {
        if (currentTabInstance != null)
        {
            Destroy(currentTabInstance);
        }
    }
    private void OnDestroy()
    {
        DestroyTab();
    }
}