using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class OrderFormatter : MonoBehaviour
{
    public string FormatOrder(Order order)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append($"{order.quantity}x ");

        if (order.ingredientes[0] is Food f)
        {
            if (f.category == FoodCategory.Sanduiche)
            {
                sb.Append("Sanduíche de " + f.ingredientName);
                if (order.ingredientes.Count > 1) sb.Append(" com " + order.ingredientes[1].ingredientName);
            }
            else if (f.category == FoodCategory.Porcao)
            {
                sb.Append("Porção de " + f.ingredientName);
                if (order.ingredientes.Count > 1) sb.Append(" com " + order.ingredientes[1].ingredientName);
            }
            else
            {
                sb.Append(f.ingredientName);
            }
        }
        else if (order.ingredientes[0] is Beverage b)
        {
            if (b.category == BeverageCategory.Juice)
            {
                sb.Append("Suco com Agua");
                foreach (var ing in order.ingredientes) sb.Append(", " + ing.ingredientName);
            }
            else if (b.category == BeverageCategory.Drink)
            {
                sb.Append($"Drink com {b.ingredientName} [{b.strength}]");
                for (int i = 1; i < order.ingredientes.Count; i++)
                {
                    sb.Append(i == order.ingredientes.Count - 1 ? " e " : ", ");
                    sb.Append(order.ingredientes[i].ingredientName);
                }
            }
            else
            {
                sb.Append(b.ingredientName);
            }
        }
        else
        {
            sb.Append(order.ingredientes[0].ingredientName);
        }

        return sb.ToString();
    }
}
