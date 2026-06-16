using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class SwapDrinkFoodModifier : OrderDebuff
{
    public SwapDrinkFoodModifier()
    {
        Name = "Senhorinha";
    }

    public override TabRules ModifySpawnRules(TabRules defaultRules)
    {
        return new TabRules(
            minOrders: Mathf.Max(2, defaultRules.maxOrders - 1),
            maxOrders: defaultRules.maxOrders,
            maxQuantity: defaultRules.maxQuantity,
            veganChance: 0.5f,
            foodOnlyChance: 0.1f,
            drinkOnlyChance: 0.1f,
            mixedChance: 0.8f,
            sidelessChance: 0.05f,
            extraSideChance: 0.25f,
            varySideCategory: true
        );
    }
    public override Tab ApplyDebuffToTab(Tab originalTab, List<Food> foodIngredients, List<Beverage> beverageIngredients)
    {
        Tab fakeTab = CopyTab(originalTab);

        Order foodOrder = FindFoodOrder(fakeTab);
        Order drinkOrder = FindDrinkOrder(fakeTab);

        if (!CanApplyDebuff(fakeTab, foodOrder, drinkOrder) || !TrySwapSides(foodOrder, drinkOrder))
        {
            CancelModifier(originalTab);
            return originalTab;
        }

        fakeTab.name = "Comanda Senhora";
        return fakeTab;
    }
    private bool TrySwapSides(Order foodOrder, Order drinkOrder)
    {
        Ingredient foodSide = FindSwappable(foodOrder);
        Ingredient drinkSide = FindSwappable(drinkOrder);

        if (foodSide == null || drinkSide == null) return false;

        foodOrder.ingredientes[foodOrder.ingredientes.IndexOf(foodSide)] = drinkSide;
        drinkOrder.ingredientes[drinkOrder.ingredientes.IndexOf(drinkSide)] = foodSide;
        return true;
    }

    private Order FindFoodOrder(Tab tab)
    {
        return tab.pedidos.FirstOrDefault(o => o.ingredientes.Any(i => i is Food f && f.type.HasFlag(IngredientFlags.Principal) && f.category != FoodCategory.Fruta) && o.ingredientes.Count >= 2);
    }

    private Order FindDrinkOrder(Tab tab)
    {
        return tab.pedidos.FirstOrDefault(o => o.ingredientes.Any(i => i is Beverage b && b.type.HasFlag(IngredientFlags.Principal)) && o.ingredientes.Count >= 2);
    }

    private Ingredient FindSwappable(Order order)
    {
        Ingredient side = order.ingredientes.FirstOrDefault(i => i.type.HasFlag(IngredientFlags.Acompanhamento));
        if (side != null) return side;

        return order.ingredientes.FirstOrDefault(i => !i.type.HasFlag(IngredientFlags.Principal));
    }

    private bool CanApplyDebuff(Tab tab, Order foodOrder, Order drinkOrder)
    {
        return tab.pedidos.Count >= 2 && foodOrder != null && drinkOrder != null;
    }

    private Tab CopyTab(Tab original)
    {
        Tab copy = ScriptableObject.CreateInstance<Tab>(); copy.pedidos = original.pedidos.Select(o => new Order(o.quantity, new List<Ingredient>(o.ingredientes))).ToList();
        return copy;
    }
    private Tab CancelModifier(Tab tab)
    {
        tab.name = "Comanda Normal(Senhora)";
        return tab;
    }
}