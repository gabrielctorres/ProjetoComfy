using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class OrderFormatter : MonoBehaviour
{
    // Passamos o índice do pedido (orderIdx) para gerar links exclusivos por linha
    public string FormatOrder(Order order, int orderIdx)
    {
        StringBuilder sb = new StringBuilder();

        if (order.ingredientes == null || order.ingredientes.Count == 0)
            return sb.ToString();

        string qtyLinkId = $"qty_{orderIdx}";
        sb.Append($"<link=\"{qtyLinkId}\">{order.quantity}x</link> ");

        string ingLinkId = $"ing_{orderIdx}";
        sb.Append($"<link=\"{ingLinkId}\">");

        Ingredient baseIng = order.ingredientes[0];

        if (baseIng is Food f)
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
        else if (baseIng is Beverage b)
        {
            if (b.category == BeverageCategory.Juice)
            {
                sb.Append("Suco com Agua");
                foreach (Ingredient ing in order.ingredientes) sb.Append(", " + ing.ingredientName);
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
            sb.Append(baseIng.ingredientName);
        }

        sb.Append("</link>");

        return sb.ToString();
    }
}