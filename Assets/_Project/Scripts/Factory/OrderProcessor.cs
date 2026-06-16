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
        Debug.Log($"[OrderProcessor] Debuff atualizado: {(newDebuff != null ? newDebuff.Name : "null")}");
    }

    public Tab ProcessTab(Tab originalTab)
    {
        Debug.Log($"[OrderProcessor] ProcessTab chamado. currentActiveDebuff = {(currentActiveDebuff != null ? currentActiveDebuff.Name : "null")}");

        if (factoryTab == null)
        {
            Debug.LogError("[OrderProcessor] factoryTab não está atribuído no Inspector!");
            return originalTab;
        }

        Debug.Log($"[OrderProcessor] Ingredientes disponíveis - Comida: {factoryTab.foodIngredients.Count}, Bebida: {factoryTab.beverageIngredients.Count}");

        Tab processedTab = originalTab;

        if (currentActiveDebuff != null)
        {
            processedTab = currentActiveDebuff.ApplyDebuffToTab(processedTab, factoryTab.foodIngredients, factoryTab.beverageIngredients);
        }

        return processedTab;
    }

    public void ConcluirPedido()
    {
        Debug.Log("Pedido finalizado com sucesso");
        OnOrderFinished?.Invoke();
    }
}