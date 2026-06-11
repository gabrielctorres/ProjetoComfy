using System;
using System.Collections.Generic;
using UnityEngine;

public class OrderProcessor : MonoBehaviour
{
    private List<IOrderModifier> modifiers = new List<IOrderModifier>();
    public static event Action OnOrderFinished;

    public void AddModifier(IOrderModifier modifier)
    {
        if (!modifiers.Contains(modifier))
            modifiers.Add(modifier);
    }

    public Tab ProcessTab(Tab originalTab)
    {
        Tab processedTab = originalTab;

        foreach (IOrderModifier modifier in modifiers)
        {
            processedTab = modifier.Modify(processedTab);
        }
        processedTab.name = "fakeTab";
        processedTab.isFake = true;
        return processedTab;
    }

    public void ConcluirPedido()
    {
        Debug.Log("Pedido finalizado com sucesso");
        OnOrderFinished?.Invoke();
    }
}