using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class SwapDrinkFoodModifier : OrderDebuff
{
    public SwapDrinkFoodModifier()
    {
        Name = "Senhorinha";
    }

    public override SpawnRules ModifySpawnRules(SpawnRules defaultRules)
    {
        return new SpawnRules(
            minOrders: Mathf.Max(2, defaultRules.maxOrders - 1),
            maxOrders: defaultRules.maxOrders,
            maxQuantity: defaultRules.maxQuantity,
            veganChance: 0.5f,
            foodOnlyChance: 0.1f,
            drinkOnlyChance: 0.1f,
            mixedChance: 0.8f
        );
    }

    public override Tab ApplyDebuffToTab(Tab originalTab, List<Food> foodIngredients, List<Beverage> beverageIngredients)
    {
        Tab fakeTab = CopyTab(originalTab);

        Order foodOrder = FindFoodOrder(fakeTab);
        Order drinkOrder = FindDrinkOrder(fakeTab);

        if (!CanApplyDebuff(fakeTab, foodOrder, drinkOrder))
        {
            return CancelModifier(fakeTab);
        }

        if (!TrySwapSides(foodOrder, drinkOrder))
        {
            return CancelModifier(fakeTab);
        }

        fakeTab.name = "Comanda Fake";
        fakeTab.isFake = true;
        return fakeTab;
    }

    private bool TrySwapSides(Order foodOrder, Order drinkOrder)
    {
        Ingredient foodSide = FindSide(foodOrder);
        Ingredient drinkSide = FindSide(drinkOrder);

        if (foodSide == null || drinkSide == null) return false;

        foodOrder.ingredientes[foodOrder.ingredientes.IndexOf(foodSide)] = drinkSide;
        drinkOrder.ingredientes[drinkOrder.ingredientes.IndexOf(drinkSide)] = foodSide;
        return true;
    }

    private Order FindFoodOrder(Tab tab)
    {
        return tab.pedidos.FirstOrDefault(o =>
            o.ingredientes.Any(i => i is Food f
                && f.type.HasFlag(IngredientFlags.Principal)
                && f.category != FoodCategory.Fruta));
    }

    private Order FindDrinkOrder(Tab tab)
    {
        return tab.pedidos.FirstOrDefault(o =>
            o.ingredientes.Any(i => i is Beverage b
                && b.type.HasFlag(IngredientFlags.Principal)));
    }

    private Ingredient FindSide(Order order)
    {
        return order.ingredientes.FirstOrDefault(i => i.type.HasFlag(IngredientFlags.Acompanhamento));
    }

    private bool CanApplyDebuff(Tab tab, Order foodOrder, Order drinkOrder)
    {
        return tab.pedidos.Count >= 2 && foodOrder != null && drinkOrder != null;
    }

    private Tab CopyTab(Tab original)
    {
        Tab copy = ScriptableObject.CreateInstance<Tab>();
        copy.pedidos = original.pedidos
            .Select(o => new Order(o.quantity, new List<Ingredient>(o.ingredientes)))
            .ToList();
        return copy;
    }

    private Tab CancelModifier(Tab tab)
    {
        tab.name = "Comanda Normal";
        tab.isFake = false;
        return tab;
    }
}