using UnityEngine;
using System.Collections.Generic;

public class ChangeQuantityModifier : IOrderModifier
{

    private int maxQuantityChange = 2;
    private bool allowDecrease = true;

    public Tab Modify(Tab originalTab)
    {
        Tab modifiedTab = ScriptableObject.CreateInstance<Tab>();
        modifiedTab.pedidos = new List<Order>();

        foreach (Order order in originalTab.pedidos)
        {
            modifiedTab.pedidos.Add(new Order(order.quantity, new List<Ingredient>(order.ingredientes)));
        }

        if (modifiedTab.pedidos.Count == 0) return modifiedTab;

        int randomOrderIndex = Random.Range(0, modifiedTab.pedidos.Count);
        Order targetOrder = modifiedTab.pedidos[randomOrderIndex];


        int minChange = allowDecrease ? -maxQuantityChange : 1;
        int quantityDelta = Random.Range(minChange, maxQuantityChange + 4);


        while (quantityDelta == 0)
        {
            quantityDelta = Random.Range(minChange, maxQuantityChange + 1);
        }


        int originalQuantity = targetOrder.quantity;
        targetOrder.quantity = Mathf.Max(1, targetOrder.quantity + quantityDelta);


        if (targetOrder.quantity == originalQuantity)
        {
            targetOrder.quantity += 1;
        }
        return modifiedTab;
    }
}