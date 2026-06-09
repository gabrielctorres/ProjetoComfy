using System.Collections.Generic;
using UnityEngine;

public class FactoryTab : MonoBehaviour
{
    [Header("Banco de Dados Único")]
    public List<Ingredient> allIngredients = new List<Ingredient>();

    [Header("Configurações de Spawn")]
    public GameObject tabPrefab;
    public Transform spawnPoint;
    int count = 0;

    [Header("Configurações da Comanda")]
    public int minOrdersPerTab = 1;
    public int maxOrdersPerTab = 4;
    public int maxQuantityPerOrder = 3;

    public void Start()
    {
        for (int i = 0; i < 5; i++) CreateTab();
    }

    public void CreateTab()
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

        if (newTab.pedidos.Count == 0) return;

        GameObject tabInstance = Instantiate(tabPrefab, spawnPoint.position, Quaternion.identity, spawnPoint);
        tabInstance.GetComponent<TabInteract>().tabData = newTab;
        tabInstance.GetComponent<TabInteract>().count = count;
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

        // Se o prato já começou a ser montado, precisamos checar a compatibilidade vegana
        if (currentList.Count > 0)
        {
            // Descobre o estado atual do prato baseado no primeiro ingrediente colocado
            bool isOrderVegano = currentList[0].type.HasFlag(IngredientFlags.Vegano);
            bool isNewIngredientVegano = ingredient.type.HasFlag(IngredientFlags.Vegano);

            // Se um for vegano e o outro não for, eles não batem. Bloqueia!
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