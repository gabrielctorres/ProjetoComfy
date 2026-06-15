using System;
using UnityEngine;
using TMPro;
public class TabInteract : MonoBehaviour, IInteractable
{
    public Tab fakeTab;
    public Tab originalTab;
    public TextMeshProUGUI tabText;
    public int count;
    public static event Action<Tab, Tab> OnTabOpen;

    public void Start()
    {
        tabText.text = $"{count}";
    }
    public void Interact()
    {
        OnTabOpen?.Invoke(this.fakeTab, this.originalTab);
    }
}
