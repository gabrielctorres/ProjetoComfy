using System.Collections.Generic;
using UnityEngine;

public class FactoryTab : MonoBehaviour
{
    [Header("Banco de Dados Único")]
    public List<Ingredient> allIngredients = new List<Ingredient>();

    [Header("Configurações da Comanda")]
    public int minOrdersPerTab = 1;
    public int maxOrdersPerTab = 4;
    public int maxQuantityPerOrder = 3;
    int count = 0;

    public Tab CreateTab()
    {
        count++;
        Tab newTab = ScriptableObject.CreateInstance<Tab>();
        newTab.pedidos = new List<Order>();

        int totalOrdersToGenerate = Random.Range(minOrdersPerTab, maxOrdersPerTab + 1);

        for (int i = 0; i < totalOrdersToGenerate; i++)
        {
            IngredientFlags chosenType = (Random.value > 0.5f) ? IngredientFlags.Comida : IngredientFlags.Bebida;
            int randomQuantity = Random.Range(1, maxQuantityPerOrder + 1);

            Order novoPrato = GenerateSingleOrder(chosenType, randomQuantity);

            if (novoPrato != null)
            {
                newTab.pedidos.Add(novoPrato);
            }
        }

        // Alternativa caso a tab esteja vazia
        if (newTab.pedidos.Count == 0)
        {
            Debug.Log("FactoryTab: Tab vazia detectada. Criando pedido padrão.");
            newTab.pedidos.Add(CreateDefaultOrder());
        }

        return newTab;
    }

    private Order CreateDefaultOrder()
    {
        // Pega o primeiro ingrediente disponível de cada tipo para garantir um pedido válido
        Ingredient principal = allIngredients.Find(i => i != null && i.type.HasFlag(IngredientFlags.Principal));
        Ingredient secundario = allIngredients.Find(i => i != null && i.type.HasFlag(IngredientFlags.Secundario));
        Ingredient acompanhante = allIngredients.Find(i => i != null && i.type.HasFlag(IngredientFlags.Acompanhamento));

        List<Ingredient> ingredients = new List<Ingredient>();
        if (principal != null) ingredients.Add(principal);
        if (secundario != null) ingredients.Add(secundario);
        if (acompanhante != null) ingredients.Add(acompanhante);

        // Se ainda não achou nada, pega o primeiro da lista geral
        if (ingredients.Count == 0 && allIngredients.Count > 0)
        {
            ingredients.Add(allIngredients[0]);
        }

        return new Order(1, ingredients);
    }

    private Order GenerateSingleOrder(IngredientFlags targetType, int quantity)
    {
        List<Ingredient> selectedIngredients = new List<Ingredient>();
        IngredientFlags orderFlags = IngredientFlags.None;

        Ingredient principal = GetRandomIngredientMatching(targetType | IngredientFlags.Principal);
        TryAddIngredient(principal, selectedIngredients, ref orderFlags, targetType);

        Ingredient secundario = GetRandomIngredientMatching(targetType | IngredientFlags.Secundario);
        TryAddIngredient(secundario, selectedIngredients, ref orderFlags, targetType);

        Ingredient acompanhante = GetRandomIngredientMatching(targetType | IngredientFlags.Acompanhamento);
        TryAddIngredient(acompanhante, selectedIngredients, ref orderFlags, targetType);

        if (selectedIngredients.Count > 0)
        {
            return new Order(quantity, selectedIngredients);
        }

        return null;
    }

    private bool TryAddIngredient(Ingredient ingredient, List<Ingredient> currentList, ref IngredientFlags orderFlags, IngredientFlags targetType)
    {
        if (ingredient == null) return false;
        if (!ingredient.type.HasFlag(targetType)) return false;

        if (currentList.Count > 0)
        {
            bool isOrderVegano = currentList[0].type.HasFlag(IngredientFlags.Vegano);
            bool isNewIngredientVegano = ingredient.type.HasFlag(IngredientFlags.Vegano);

            if (isOrderVegano != isNewIngredientVegano) return false;
        }

        currentList.Add(ingredient);
        orderFlags |= ingredient.type;
        return true;
    }

    private Ingredient GetRandomIngredientMatching(IngredientFlags requiredFlags)
    {
        if (allIngredients == null || allIngredients.Count == 0) return null;

        List<Ingredient> validOptions = allIngredients.FindAll(ing => ing != null && ing.type.HasFlag(requiredFlags));

        if (validOptions.Count == 0) return null;
        return validOptions[Random.Range(0, validOptions.Count)];
    }
}
