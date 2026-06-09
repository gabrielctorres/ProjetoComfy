using TMPro;
using UnityEngine;

public class TabView : MonoBehaviour
{
    //public Tab currentTab;

    // vai ouvir o evento
    public GameObject containerTab;
    public TextMeshProUGUI textTab;

    public void OnEnable()
    {
        TabInteract.OnTabOpen += OpenTab;
        textTab.text = "";
    }

    public void OnDisable()
    {
        TabInteract.OnTabOpen -= OpenTab;
    }

    private void OpenTab(Tab tabData)
    {
        containerTab.SetActive(true);
        foreach (var pedido in tabData.pedidos)
        {
            textTab.text += FormatterTab.FormatPedido(pedido) + "\n";
        }
    }
}
