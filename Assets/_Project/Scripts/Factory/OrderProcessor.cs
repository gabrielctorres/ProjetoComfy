using System;
using System.Collections.Generic;
using UnityEngine;

public class OrderProcessor : MonoBehaviour
{
    [Header("Configurações de Ingredientes")]
    [SerializeField] private FactoryTab factoryTab;

    private OrderDebuff currentActiveDebuff;

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

    public Tab ProcessTab(Tab originalTab)
    {

        if (factoryTab == null)
        {
            return originalTab;
        }


        Tab processedTab = originalTab;

        if (currentActiveDebuff != null)
        {
            processedTab = currentActiveDebuff.ApplyDebuffToTab(processedTab, factoryTab.foodIngredients, factoryTab.beverageIngredients);
        }

        return processedTab;
    }

    public void ConcluirPedido()
    {
        OnOrderFinished?.Invoke();
    }
}