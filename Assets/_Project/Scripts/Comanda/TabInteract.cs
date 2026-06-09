using System;
using UnityEngine;
using TMPro;
public class TabInteract : MonoBehaviour, IInteractable
{
    public Tab tabData;
    public TextMeshProUGUI tabText;
    public int count;
    public static event Action<Tab> OnTabOpen;

    public void Start()
    {
        tabText.text = $"{count}";
    }
    public void Interact()
    {
        // Disparar um evento para abrir a tab
        OnTabOpen?.Invoke(this.tabData);
    }
}
