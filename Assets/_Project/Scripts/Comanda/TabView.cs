using TMPro;
using UnityEngine;

public class TabView : MonoBehaviour
{
    public bool isOpen;
    public GameObject containerTab;
    public TextMeshProUGUI textTab;
    public TextMeshProUGUI typeText;
    private OrderFormatter orderFormatter;

    private Tab fakeTab;
    private Tab originalTab;

    void Start()
    {
        textTab.text = "";
        orderFormatter = GetComponent<OrderFormatter>();
    }

    public void OnEnable()
    {
        TabInteract.OnTabOpen += OpenTab;
        WaiterSpawner.OnTabRefreshed += RefreshTabVisuals;
    }

    public void OnDisable()
    {
        TabInteract.OnTabOpen -= OpenTab;
        WaiterSpawner.OnTabRefreshed -= RefreshTabVisuals;
    }

    private void OpenTab(Tab fakeTab, Tab originalTab)
    {
        if (isOpen) return;

        this.fakeTab = fakeTab;
        this.originalTab = originalTab;

        typeText.text = fakeTab.name;

        containerTab.SetActive(true);
        isOpen = true;

        RefreshTabVisuals();
    }

    public void RefreshTabVisuals()
    {
        if (fakeTab == null || !isOpen) return;

        textTab.text = "";

        for (int i = 0; i < fakeTab.pedidos.Count; i++)
        {
            string formatted = orderFormatter.FormatOrder(fakeTab.pedidos[i], i, originalTab, fakeTab);
            textTab.text += formatted + "\n";
        }
    }

    public void CloseTab()
    {
        containerTab.SetActive(false);
        textTab.text = "";
        fakeTab = null;
        originalTab = null;
        isOpen = false;
    }
}