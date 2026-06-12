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
            mixedChance: 0.8f);
    }

    public override Tab ApplyDebuffToTab(Tab originalTab, List<Food> foodIngredients, List<Beverage> beverageIngredients)
    {
        Tab fakeTab = ScriptableObject.CreateInstance<Tab>();
        fakeTab.pedidos = new List<Order>();

        foreach (Order order in originalTab.pedidos)
        {
            fakeTab.pedidos.Add(new Order(order.quantity, new List<Ingredient>(order.ingredientes)));
        }

        if (fakeTab.pedidos.Count < 2)
        {
            fakeTab.name = "Comanda Bagunçada (Fake)";
            fakeTab.isFake = true;
            return fakeTab;
        }

        List<Ingredient> todosPrincipais = new List<Ingredient>();
        List<Ingredient> todosAcompanhamentos = new List<Ingredient>();

        foreach (Order order in fakeTab.pedidos)
        {
            foreach (Ingredient ing in order.ingredientes)
            {
                if (ing.type.HasFlag(IngredientFlags.Principal))
                    todosPrincipais.Add(ing);
                else if (ing.type.HasFlag(IngredientFlags.Acompanhamento))
                    todosAcompanhamentos.Add(ing);
            }
        }

        RotacionarLista(todosPrincipais);
        RotacionarLista(todosAcompanhamentos);

        int indexPrincipal = 0;
        int indexAcompanhamento = 0;

        foreach (Order order in fakeTab.pedidos)
        {
            List<Ingredient> novosIngredientes = new List<Ingredient>();

            foreach (Ingredient ing in order.ingredientes)
            {
                if (ing.type.HasFlag(IngredientFlags.Principal) && todosPrincipais.Count > 0)
                {
                    novosIngredientes.Add(todosPrincipais[indexPrincipal]);
                    indexPrincipal = (indexPrincipal + 1) % todosPrincipais.Count;
                }
                else if (ing.type.HasFlag(IngredientFlags.Acompanhamento) && todosAcompanhamentos.Count > 0)
                {
                    novosIngredientes.Add(todosAcompanhamentos[indexAcompanhamento]);
                    indexAcompanhamento = (indexAcompanhamento + 1) % todosAcompanhamentos.Count;
                }
                else
                {
                    novosIngredientes.Add(ing);
                }
            }

            order.ingredientes = novosIngredientes;
        }

        fakeTab.name = "Comanda Fake";
        fakeTab.isFake = true;
        return fakeTab;
    }

    private void RotacionarLista(List<Ingredient> lista)
    {
        if (lista.Count < 2) return;

        Ingredient primeiro = lista[0];
        lista.RemoveAt(0);
        lista.Add(primeiro);
    }
}