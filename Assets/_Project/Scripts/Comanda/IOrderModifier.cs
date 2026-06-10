using System.Collections.Generic;
using UnityEngine;

public interface IOrderModifier
{
    Order Modify(Order originalOrder);
}

public class SwapDrinkFoodModifier : IOrderModifier
{
    public Order Modify(Order originalOrder)
    {
        List<Ingredient> ingredients = new List<Ingredient>(originalOrder.ingredientes);

        int drinkIndex = -1;
        int foodIndex = -1;

        for (int i = 0; i < ingredients.Count; i++)
        {
            if (ingredients[i].type.HasFlag(IngredientFlags.Bebida))
                drinkIndex = i;

            if (ingredients[i].type.HasFlag(IngredientFlags.Comida))
                foodIndex = i;
        }

        if (drinkIndex != -1 && foodIndex != -1)
        {
            Ingredient temp = ingredients[drinkIndex];
            ingredients[drinkIndex] = ingredients[foodIndex];
            ingredients[foodIndex] = temp;
        }

        Order modifiedOrder = new Order(originalOrder.quantity, ingredients);
        return modifiedOrder;
    }
}