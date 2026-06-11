using TMPro;
using UnityEngine;

public class TabView : MonoBehaviour
{
    public bool isOpen;
    public GameObject containerTab;
    public TextMeshProUGUI textTab;
    public TextMeshProUGUI typeText;
    private OrderFormatter orderFormatter;

    private Tab currentOpenTabData;

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

    private void OpenTab(Tab tabData)
    {
        if (isOpen) return;

        currentOpenTabData = tabData;

        if (tabData.isFake)
        {
            typeText.text = "Fake Tab";
        }
        else
        {
            typeText.text = "Real Tab";
        }

        containerTab.SetActive(true);
        isOpen = true;

        RefreshTabVisuals();
    }

    public void RefreshTabVisuals()
    {
        if (currentOpenTabData == null || !isOpen) return;

        textTab.text = "";

        for (int i = 0; i < currentOpenTabData.pedidos.Count; i++)
        {
            string formatted = orderFormatter.FormatOrder(currentOpenTabData.pedidos[i], i);
            textTab.text += formatted + "\n";
        }
    }

    public void CloseTab()
    {
        containerTab.SetActive(false);
        textTab.text = "";
        currentOpenTabData = null;
        isOpen = false;
    }
}