using UnityEngine;
using System.Collections.Generic;

public class ChangeQuantityModifier : OrderDebuff
{
    private int maxQuantityChange = 2;
    private bool allowDecrease = true;

    public ChangeQuantityModifier()
    {
        Name = "Discalculia";
    }

    public override Tab ApplyDebuffToTab(Tab originalTab, List<Food> foodIngredients, List<Beverage> beverageIngredients)
    {
        Tab fakeTab = ScriptableObject.CreateInstance<Tab>();
        fakeTab.pedidos = new List<Order>();

        foreach (Order order in originalTab.pedidos)
        {
            fakeTab.pedidos.Add(new Order(order.quantity, new List<Ingredient>(order.ingredientes)));
        }

        if (fakeTab.pedidos.Count == 0)
        {
            return fakeTab;
        }

        int randomOrderIndex = Random.Range(0, fakeTab.pedidos.Count);
        Order targetOrder = fakeTab.pedidos[randomOrderIndex];

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

        fakeTab.name = "Comanda Fake";
        return fakeTab;
    }
}