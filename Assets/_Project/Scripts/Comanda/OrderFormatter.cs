using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class OrderFormatter : MonoBehaviour
{
    public string FormatOrder(Order fakeOrder, int orderIdx, Tab originalTab, Tab fakeTab)
    {
        StringBuilder sb = new StringBuilder();

        if (fakeOrder.ingredientes == null || fakeOrder.ingredientes.Count == 0)
            return sb.ToString();

        bool isRealTab = (originalTab == fakeTab);

        bool isQtyWrong = false;
        if (originalTab != null && orderIdx < originalTab.pedidos.Count)
        {
            Order originalOrder = originalTab.pedidos[orderIdx];
            isQtyWrong = fakeOrder.quantity != originalOrder.quantity;
        }

        bool underlineQty = isQtyWrong;

        List<bool> underlineIng = new List<bool>();
        for (int i = 0; i < fakeOrder.ingredientes.Count; i++)
        {
            underlineIng.Add(true);
        }

        Random.State oldState = Random.state;
        Random.InitState(fakeTab.GetInstanceID() + orderIdx);

        if (isRealTab)
        {
            if (Random.value < 0.35f) underlineQty = true;
        }
        else
        {
            if (!isQtyWrong && Random.value < 0.25f) underlineQty = true;
        }

        Random.state = oldState;

        string qtyLinkId = $"qty_{orderIdx}";
        string qtyText = $"{fakeOrder.quantity}x";
        if (underlineQty) qtyText = $"<u>{qtyText}</u>";

        sb.Append($"<link=\"{qtyLinkId}\">{qtyText}</link> ");

        Ingredient baseIng = fakeOrder.ingredientes[0];

        if (baseIng is Food f)
        {
            if (f.category == FoodCategory.Sanduiche)
            {
                // Em inglês: [Ingrediente] Sandwich (Ex: "Chicken Sandwich")
                AppendIngredientLink(sb, orderIdx, 0, fakeOrder.ingredientes[0].ingredientName, underlineIng[0]);
                sb.Append(" Sandwich");

                if (fakeOrder.ingredientes.Count > 1)
                {
                    sb.Append(" with ");
                    AppendIngredientLink(sb, orderIdx, 1, fakeOrder.ingredientes[1].ingredientName, underlineIng[1]);
                }
            }
            else if (f.category == FoodCategory.Porcao)
            {
                sb.Append("Portion of ");
                AppendIngredientLink(sb, orderIdx, 0, fakeOrder.ingredientes[0].ingredientName, underlineIng[0]);

                if (fakeOrder.ingredientes.Count > 1)
                {
                    sb.Append(" with ");
                    AppendIngredientLink(sb, orderIdx, 1, fakeOrder.ingredientes[1].ingredientName, underlineIng[1]);
                }
            }
            else
            {
                AppendIngredientLink(sb, orderIdx, 0, f.ingredientName, underlineIng[0]);
            }
        }
        else if (baseIng is Beverage b)
        {
            if (b.category == BeverageCategory.Juice)
            {
                AppendIngredientLink(sb, orderIdx, 0, fakeOrder.ingredientes[0].ingredientName, underlineIng[0]);
                sb.Append(" Juice");

                for (int i = 1; i < fakeOrder.ingredientes.Count; i++)
                {
                    sb.Append(", ");
                    AppendIngredientLink(sb, orderIdx, i, fakeOrder.ingredientes[i].ingredientName, underlineIng[i]);
                }
            }
            else if (b.category == BeverageCategory.Drink)
            {
                AppendIngredientLink(sb, orderIdx, 0, fakeOrder.ingredientes[0].ingredientName, underlineIng[0]);
                sb.Append($" Drink [{b.strength}]");

                for (int i = 1; i < fakeOrder.ingredientes.Count; i++)
                {
                    sb.Append(i == fakeOrder.ingredientes.Count - 1 ? " and " : ", ");
                    AppendIngredientLink(sb, orderIdx, i, fakeOrder.ingredientes[i].ingredientName, underlineIng[i]);
                }
            }
            else
            {
                AppendIngredientLink(sb, orderIdx, 0, b.ingredientName, underlineIng[0]);
            }
        }
        else
        {
            AppendIngredientLink(sb, orderIdx, 0, baseIng.ingredientName, underlineIng[0]);
        }

        return sb.ToString();
    }

    private void AppendIngredientLink(StringBuilder sb, int orderIdx, int ingIdx, string ingredientName, bool underline)
    {
        string text = underline ? $"<u>{ingredientName}</u>" : ingredientName;
        sb.Append($"<link=\"ing_{orderIdx}_{ingIdx}\">{text}</link>");
    }
}