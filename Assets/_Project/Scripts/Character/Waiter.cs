using System;
using UnityEngine;
using DG.Tweening; // Importante para o DOTween

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
            interact.fakeTab = fakeTab; interact.originalTab = originalTab;
        }

        currentTabInstance.transform.localScale = Vector3.zero;
        currentTabInstance.transform.DOScale(Vector3.one, 0.25f).SetEase(Ease.OutQuad);
    }


    public void AnimateExitAndDestroy()
    {
        if (currentTabInstance != null)
        {
            currentTabInstance.transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InBack);
        }

        transform.DOScale(Vector3.zero, 0.25f).SetEase(Ease.InBack);
        transform.DOMoveY(transform.position.y - 0.5f, 0.25f).SetEase(Ease.InQuad).OnComplete(() =>
        {
            Destroy(gameObject);
        });
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
        transform.DOKill();
        if (currentTabInstance != null) currentTabInstance.transform.DOKill();

        DestroyTab();
    }
}
[System.Serializable]
public class WaiterData
{
    public Tab originalTab;
    public Tab fakeTab;
    public bool isLiar;
}