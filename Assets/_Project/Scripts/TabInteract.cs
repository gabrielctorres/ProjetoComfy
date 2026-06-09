using System;
using UnityEngine;

public class TabInteract : MonoBehaviour, IInteractable
{
    public Tab tabData;
    public static event Action<Tab> OnTabOpen;

    public void Interact()
    {
        // Disparar um evento para abrir a tab
        OnTabOpen?.Invoke(this.tabData);
    }
    public void Resolve()
    {
        throw new NotImplementedException();
    }

    public void Close()
    {
        throw new NotImplementedException();
    }
}
