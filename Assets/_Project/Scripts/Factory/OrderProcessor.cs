using System.Collections.Generic;
using UnityEngine;

public class OrderProcessor : MonoBehaviour
{
    private List<IOrderModifier> activeModifiers = new List<IOrderModifier>();

    public void AddModifier(IOrderModifier modifier)
    {
        if (!activeModifiers.Contains(modifier))
        {
            activeModifiers.Add(modifier);
        }
    }

    public Tab ProcessTab(Tab originalTab)
    {
        Tab fakeTab = ScriptableObject.CreateInstance<Tab>();
        fakeTab.name = "fakeTab";
        fakeTab.pedidos = new List<Order>();

        if (originalTab.pedidos == null)
        {
            return fakeTab;
        }

        foreach (var order in originalTab.pedidos)
        {
            Order fakeOrder = new Order(order.quantity, new List<Ingredient>(order.ingredientes));

            foreach (var mod in activeModifiers)
            {
                fakeOrder = mod.Modify(fakeOrder);
            }
            fakeTab.pedidos.Add(fakeOrder);
        }
        return fakeTab;
    }

    public Order Process(Order order)
    {
        Order finalOrder = order;
        foreach (var mod in activeModifiers)
        {
            finalOrder = mod.Modify(finalOrder);
        }
        return finalOrder;
    }
}
