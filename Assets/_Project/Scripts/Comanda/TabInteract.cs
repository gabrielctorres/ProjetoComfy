using System;
using UnityEngine;
using TMPro;
public class TabInteract : MonoBehaviour, IInteractable
{
    public Tab tabData;
    public Tab originalData;


    public TextMeshProUGUI tabText;
    public int count;
    public static event Action<Tab> OnTabOpen;

    public void Start()
    {
        tabText.text = $"{count}";
    }
    public void Interact()
    {
        OnTabOpen?.Invoke(this.tabData);
    }
}
