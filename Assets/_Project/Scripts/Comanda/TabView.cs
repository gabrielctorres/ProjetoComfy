using TMPro;
using UnityEngine;

public class TabView : MonoBehaviour
{
    public bool isOpen;
    public GameObject containerTab;
    public TextMeshProUGUI textTab;
    public TextMeshProUGUI typeText;
    private OrderFormatter orderFormatter;

    void Start()
    {
        textTab.text = "";
        orderFormatter = GetComponent<OrderFormatter>();

    }
    public void OnEnable()
    {
        TabInteract.OnTabOpen += OpenTab;

    }

    public void OnDisable()
    {
        TabInteract.OnTabOpen -= OpenTab;
    }

    private void OpenTab(Tab tabData)
    {
        if (isOpen) return;
        if (tabData.isFake) // temporario
        {
            typeText.text = "Fake Tab";
        }
        else
        {
            typeText.text = "Real Tab";
        }
        containerTab.SetActive(true);
        for (int i = 0; i < tabData.pedidos.Count; i++)
        {
            string formatted = orderFormatter.FormatPedido(tabData.pedidos[i], i);
            textTab.text += formatted + "\n";
        }

        isOpen = true;
    }
    public void CloseTab()
    {
        containerTab.SetActive(false);
        textTab.text = "";
        isOpen = false;
    }
}
