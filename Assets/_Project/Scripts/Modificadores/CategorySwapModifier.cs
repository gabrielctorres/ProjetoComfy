using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class CategorySwapModifier : OrderDebuff
{
    public CategorySwapModifier()
    {
        Name = "Crise de Identidade";
    }

    public override Tab ApplyDebuffToTab(Tab originalTab, List<Food> foodIngredients, List<Beverage> beverageIngredients)
    {
        Tab fakeTab = CopyTab(originalTab);

        if (fakeTab.pedidos.Count == 0)
        {
            return originalTab;
        }

        int randomOrderIdx = Random.Range(0, fakeTab.pedidos.Count);
        Order targetOrder = fakeTab.pedidos[randomOrderIdx];

        Ingredient principal = targetOrder.ingredientes.FirstOrDefault(i => i.type.HasFlag(IngredientFlags.Principal));

        if (principal == null)
        {
            return originalTab;
        }

        if (principal is Food foodPrincipal)
        {
            var differentCategoryFood = foodIngredients
                .Where(f => f.category != foodPrincipal.category && f.type.HasFlag(IngredientFlags.Principal))
                .ToList();

            if (differentCategoryFood.Count > 0)
            {
                int idx = targetOrder.ingredientes.IndexOf(principal);
                Ingredient oldIng = targetOrder.ingredientes[idx];
                targetOrder.ingredientes[idx] = differentCategoryFood[Random.Range(0, differentCategoryFood.Count)];
            }
            else
            {
                return originalTab;
            }
        }
        else if (principal is Beverage beveragePrincipal)
        {
            var differentCategoryBeverage = beverageIngredients.Where(b => b.category != beveragePrincipal.category && b.type.HasFlag(IngredientFlags.Principal)).ToList();

            if (differentCategoryBeverage.Count > 0)
            {
                int idx = targetOrder.ingredientes.IndexOf(principal);
                Ingredient oldIng = targetOrder.ingredientes[idx];
                targetOrder.ingredientes[idx] = differentCategoryBeverage[Random.Range(0, differentCategoryBeverage.Count)];
            }
            else
            {
                return originalTab;
            }
        }

        fakeTab.name = "Comanda Fake (Categoria)";
        return fakeTab;
    }

    private Tab CopyTab(Tab original)
    {
        Tab copy = ScriptableObject.CreateInstance<Tab>();
        copy.pedidos = original.pedidos.Select(o => new Order(o.quantity, new List<Ingredient>(o.ingredientes))).ToList();
        return copy;
    }
}
