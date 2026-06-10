using UnityEngine;
using System.Collections.Generic;
using System.Linq;

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

        var principalIngredients = new List<(int orderIdx, int ingIdx, bool isVegan)>();
        var acompanhamentoIngredients = new List<(int orderIdx, int ingIdx, bool isVegan)>();

        for (int o = 0; o < modifiedTab.pedidos.Count; o++)
        {
            List<Ingredient> ings = modifiedTab.pedidos[o].ingredientes;
            for (int i = 0; i < ings.Count; i++)
            {
                if (ings[i].type.HasFlag(IngredientFlags.Principal))
                    principalIngredients.Add((o, i, ings[i].isVegan));
                else if (ings[i].type.HasFlag(IngredientFlags.Acompanhamento))
                    acompanhamentoIngredients.Add((o, i, ings[i].isVegan));
            }
        }

        TrySwap(modifiedTab, principalIngredients);
        TrySwap(modifiedTab, acompanhamentoIngredients);

        return modifiedTab;
    }

    private void TrySwap(Tab tab, List<(int orderIdx, int ingIdx, bool isVegan)> candidates)
    {
        if (candidates.Count < 2) return;

        var veganCandidates = candidates.Where(c => c.isVegan).ToList();
        var meatCandidates = candidates.Where(c => !c.isVegan).ToList();

        PerformSwap(tab, veganCandidates);
        PerformSwap(tab, meatCandidates);
    }

    private void PerformSwap(Tab tab, List<(int orderIdx, int ingIdx, bool isVegan)> list)
    {
        if (list.Count < 2) return;

        var first = list[Random.Range(0, list.Count)];
        var second = list[Random.Range(0, list.Count)];

        if (first.orderIdx == second.orderIdx && first.ingIdx == second.ingIdx) return;

        List<Ingredient> ingsFirst = tab.pedidos[first.orderIdx].ingredientes;
        List<Ingredient> ingsSecond = tab.pedidos[second.orderIdx].ingredientes;

        Ingredient temp = ingsFirst[first.ingIdx];
        ingsFirst[first.ingIdx] = ingsSecond[second.ingIdx];
        ingsSecond[second.ingIdx] = temp;
    }
}
