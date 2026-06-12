using System;
using System.Collections.Generic;
using UnityEngine;

public class OrderProcessor : MonoBehaviour
{
    [Header("Configurações de Ingredientes")]
    [SerializeField] private FactoryTab factoryTab; // Para obter as listas de ingredientes puras

    private List<IOrderModifier> modifiers = new List<IOrderModifier>();
    private OrderDebuff currentActiveDebuff; // Guardado via evento

    public static event Action OnOrderFinished;

    private void OnEnable()
    {
        DayManager.OnDebuffChanged += UpdateActiveDebuff;
    }

    private void OnDisable()
    {
        DayManager.OnDebuffChanged -= UpdateActiveDebuff;
    }

    private void UpdateActiveDebuff(OrderDebuff newDebuff)
    {
        currentActiveDebuff = newDebuff;
    }

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

        if (currentActiveDebuff != null)
        {
            processedTab = currentActiveDebuff.ApplyDebuffToTab(processedTab, factoryTab.foodIngredients, factoryTab.beverageIngredients);
        }
        else
        {
            processedTab.name = "fakeTab";
            processedTab.isFake = true;
        }

        return processedTab;
    }

    public void ConcluirPedido()
    {
        Debug.Log("Pedido finalizado com sucesso");
        OnOrderFinished?.Invoke();
    }
}