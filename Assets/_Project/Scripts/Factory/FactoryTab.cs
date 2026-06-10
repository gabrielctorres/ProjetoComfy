using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FactoryTab : MonoBehaviour
{
    public List<Food> foodIngredients;
    public List<Beverage> beverageIngredients;

    [Header("Configurações de Geração")]
    public int minOrdersPerTab = 1;
    public int maxOrdersPerTab = 4;
    public int maxQuantityPerOrder = 3;

    public Tab CreateTab()
    {
        Tab tab = ScriptableObject.CreateInstance<Tab>();
        tab.pedidos = new List<Order>();

        int orderCount = Random.Range(minOrdersPerTab, maxOrdersPerTab + 1);
        for (int i = 0; i < orderCount; i++)
        {
            tab.pedidos.Add(GenerateSingleOrder());
        }
        
        return tab;
    }

    private Order GenerateSingleOrder()
    {
        bool isFood = Random.value > 0.5f;
        bool isVegan = Random.value > 0.5f;

        List<Ingredient> selectedIngredients = new List<Ingredient>();

        if (isFood)
        {
            FoodCategory cat = (FoodCategory)Random.Range(0, 2); // Sanduiche ou Porcao
            var principalOptions = foodIngredients.Where(f => f.category == cat && f.type.HasFlag(IngredientFlags.Principal) && f.isVegan == isVegan).ToList();
            var acompanhamentoOptions = foodIngredients.Where(f => f.type.HasFlag(IngredientFlags.Acompanhamento) && f.isVegan == isVegan).ToList();

            if (principalOptions.Count > 0) selectedIngredients.Add(principalOptions[Random.Range(0, principalOptions.Count)]);
            if (acompanhamentoOptions.Count > 0) selectedIngredients.Add(acompanhamentoOptions[Random.Range(0, acompanhamentoOptions.Count)]);
        }
        else
        {
            bool isDrink = Random.value > 0.5f;
            if (!isDrink) // Suco
            {
                var frutaOptions = foodIngredients.Where(f => f.category == FoodCategory.Fruta && f.isVegan == isVegan).ToList();
                var acompanhamentoOptions = beverageIngredients.Where(b => b.category == BeverageCategory.Juice && b.type.HasFlag(IngredientFlags.Acompanhamento) && b.isVegan == isVegan).ToList();

                if (frutaOptions.Count > 0) selectedIngredients.Add(frutaOptions[Random.Range(0, frutaOptions.Count)]);
                if (acompanhamentoOptions.Count > 0) selectedIngredients.Add(acompanhamentoOptions[Random.Range(0, acompanhamentoOptions.Count)]);
            }
            else // Drink
            {
                var baseDrinkOptions = beverageIngredients.Where(b => b.category == BeverageCategory.Drink && b.type.HasFlag(IngredientFlags.Principal) && b.isVegan == isVegan).ToList();
                var frutaOptions = foodIngredients.Where(f => f.category == FoodCategory.Fruta && f.isVegan == isVegan).ToList();
                var acompanhamentoOptions = beverageIngredients.Where(b => b.category == BeverageCategory.Drink && b.type.HasFlag(IngredientFlags.Acompanhamento) && b.isVegan == isVegan).ToList();

                if (baseDrinkOptions.Count > 0) selectedIngredients.Add(baseDrinkOptions[Random.Range(0, baseDrinkOptions.Count)]);
                if (frutaOptions.Count > 0) selectedIngredients.Add(frutaOptions[Random.Range(0, frutaOptions.Count)]);
                if (acompanhamentoOptions.Count > 0) selectedIngredients.Add(acompanhamentoOptions[Random.Range(0, acompanhamentoOptions.Count)]);
            }
        }

        // Retorna o pedido gerado ou o padrão dinâmico caso esteja vazio
        int quantity = Random.Range(1, maxQuantityPerOrder + 1);
        return selectedIngredients.Count > 0 ? new Order(quantity, selectedIngredients) : CreateDefaultOrder(quantity);
    }

    private Order CreateDefaultOrder(int quantity)
    {
        var allIngredients = foodIngredients.Cast<Ingredient>().Concat(beverageIngredients.Cast<Ingredient>()).ToList();
        
        List<Ingredient> ingredients = new List<Ingredient>();
        var principal = allIngredients.FirstOrDefault(i => i != null && i.type.HasFlag(IngredientFlags.Principal));
        var acompanhante = allIngredients.FirstOrDefault(i => i != null && i.type.HasFlag(IngredientFlags.Acompanhamento));

        if (principal != null) ingredients.Add(principal);
        if (acompanhante != null) ingredients.Add(acompanhante);

        if (ingredients.Count == 0 && allIngredients.Count > 0)
        {
            ingredients.Add(allIngredients[0]);
        }

        return new Order(quantity, ingredients);
    }
}
