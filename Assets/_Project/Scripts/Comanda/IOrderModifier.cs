using System.Collections.Generic;
using UnityEngine;

public interface IOrderModifier
{
    Tab Modify(Tab originalTab);
}

public class SwapDrinkFoodModifier : IOrderModifier
{

    public Tab Modify(Tab originalTab)
    {
        Tab modifiedTab = ScriptableObject.CreateInstance<Tab>();
        modifiedTab.pedidos = new List<Order>();

        foreach (var order in originalTab.pedidos)
        {
            modifiedTab.pedidos.Add(new Order(order.quantity, new List<Ingredient>(order.ingredientes)));
        }

        List<(int orderIdx, int ingIdx)> drinkCoordinates = new List<(int, int)>();
        List<(int orderIdx, int ingIdx)> foodCoordinates = new List<(int, int)>();

        for (int o = 0; o < modifiedTab.pedidos.Count; o++)
        {
            List<Ingredient> ings = modifiedTab.pedidos[o].ingredientes;
            for (int i = 0; i < ings.Count; i++)
            {
                if (ings[i].type.HasFlag(IngredientFlags.Bebida))
                {
                    drinkCoordinates.Add((o, i));
                }
                else if (ings[i].type.HasFlag(IngredientFlags.Comida))
                {
                    foodCoordinates.Add((o, i));
                }
            }
        }
        if (drinkCoordinates.Count > 0 && foodCoordinates.Count > 0)
        {
            var targetDrink = drinkCoordinates[Random.Range(0, drinkCoordinates.Count)];
            var targetFood = foodCoordinates[Random.Range(0, foodCoordinates.Count)];

            List<Ingredient> drinkOrderIngredients = modifiedTab.pedidos[targetDrink.orderIdx].ingredientes;
            List<Ingredient> foodOrderIngredients = modifiedTab.pedidos[targetFood.orderIdx].ingredientes;

            Ingredient temp = drinkOrderIngredients[targetDrink.ingIdx];
            drinkOrderIngredients[targetDrink.ingIdx] = foodOrderIngredients[targetFood.ingIdx];
            foodOrderIngredients[targetFood.ingIdx] = temp;

            Debug.Log($"[MODIFIER] Sucesso! Trocado ingrediente entre o Pedido {targetDrink.orderIdx} e o Pedido {targetFood.orderIdx}");
        }
        else
        {
            Debug.LogWarning("[MODIFIER] A comanda não continha ambos os tipos (Bebida e Comida) para realizar a troca.");
        }

        return modifiedTab;
    }
}