using System; // Necessário para o Enum.GetValues
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FactoryTab : MonoBehaviour
{
    public List<Food> foodIngredients;
    public List<Beverage> beverageIngredients;

    private int minOrders = 1;
    private int maxOrders = 4;
    private int maxQuantity = 3;

    private SpawnRules defaultRules;

    private void Awake()
    {
        defaultRules = new SpawnRules(minOrders, maxOrders, maxQuantity);
    }

    public SpawnRules DefaultRules => defaultRules;

    public Tab CreateTab(SpawnRules customRules = null)
    {
        SpawnRules rules = customRules ?? defaultRules;

        Tab tab = ScriptableObject.CreateInstance<Tab>();
        tab.pedidos = new List<Order>();

        int orderCount = UnityEngine.Random.Range(rules.minOrders, rules.maxOrders + 1);
        for (int i = 0; i < orderCount; i++)
        {
            tab.pedidos.Add(GenerateSingleOrder(rules));
        }

        return tab;
    }

    private Order GenerateSingleOrder(SpawnRules rules)
    {
        Order order;
        int maxAttempts = 10;
        int attempts = 0;

        do
        {
            order = GenerateOrderAttempt(rules);
            attempts++;
        } while (!IsValid(order) && attempts < maxAttempts);

        return order;
    }

    private Order GenerateOrderAttempt(SpawnRules rules)
    {
        bool isVegan = UnityEngine.Random.value < rules.veganChance;
        List<Ingredient> selectedIngredients = new List<Ingredient>();

        float roll = UnityEngine.Random.value;

        if (roll < rules.foodOnlyChance)
        {
            AddFoodToIngredients(selectedIngredients, isVegan);
        }
        else if (roll < rules.foodOnlyChance + rules.drinkOnlyChance)
        {
            AddDrinkToIngredients(selectedIngredients, isVegan);
        }
        else
        {
            AddFoodToIngredients(selectedIngredients, isVegan);
            AddDrinkToIngredients(selectedIngredients, isVegan);
        }

        int quantity = UnityEngine.Random.Range(1, rules.maxQuantity + 1);
        return selectedIngredients.Count > 0 ? new Order(quantity, selectedIngredients) : CreateDefaultOrder(quantity);
    }

    private void AddFoodToIngredients(List<Ingredient> list, bool isVegan)
    {
        var categories = Enum.GetValues(typeof(FoodCategory));
        FoodCategory cat = (FoodCategory)categories.GetValue(UnityEngine.Random.Range(0, categories.Length));

        List<Food> principalOptions = foodIngredients.Where(f => f.category == cat && f.type.HasFlag(IngredientFlags.Principal) && f.isVegan == isVegan).ToList();
        List<Food> acompanhamentoOptions = foodIngredients.Where(f => f.type.HasFlag(IngredientFlags.Acompanhamento) && f.isVegan == isVegan && f.category != FoodCategory.Fruta).ToList();


        if (principalOptions.Count == 0 && !isVegan)
        {
            principalOptions = foodIngredients.Where(f => f.category == cat && f.type.HasFlag(IngredientFlags.Principal)).ToList();
        }
        if (acompanhamentoOptions.Count == 0 && !isVegan)
        {
            acompanhamentoOptions = foodIngredients.Where(f => f.type.HasFlag(IngredientFlags.Acompanhamento) && f.category != FoodCategory.Fruta).ToList();
        }

        if (principalOptions.Count > 0) list.Add(principalOptions[UnityEngine.Random.Range(0, principalOptions.Count)]);
        if (acompanhamentoOptions.Count > 0) list.Add(acompanhamentoOptions[UnityEngine.Random.Range(0, acompanhamentoOptions.Count)]);
    }

    private void AddDrinkToIngredients(List<Ingredient> list, bool isVegan)
    {
        bool isDrink = UnityEngine.Random.value > 0.5f;
        if (!isDrink)
        {
            List<Food> frutaOptions = foodIngredients.Where(f => f.category == FoodCategory.Fruta && f.isVegan == isVegan).ToList();
            List<Beverage> acompanhamentoOptions = beverageIngredients.Where(b => b.category == BeverageCategory.Juice && b.type.HasFlag(IngredientFlags.Acompanhamento) && b.isVegan == isVegan).ToList();

            if (frutaOptions.Count > 0 && acompanhamentoOptions.Count > 0)
            {
                list.Add(frutaOptions[UnityEngine.Random.Range(0, frutaOptions.Count)]);
                list.Add(acompanhamentoOptions[UnityEngine.Random.Range(0, acompanhamentoOptions.Count)]);
            }
        }
        else
        {
            List<Beverage> baseDrinkOptions = beverageIngredients.Where(b => b.category == BeverageCategory.Drink && b.type.HasFlag(IngredientFlags.Principal) && b.isVegan == isVegan).ToList();
            List<Food> frutaOptions = foodIngredients.Where(f => f.category == FoodCategory.Fruta && f.isVegan == isVegan).ToList();
            List<Beverage> acompanhamentoOptions = beverageIngredients.Where(b => b.category == BeverageCategory.Drink && b.type.HasFlag(IngredientFlags.Acompanhamento) && b.isVegan == isVegan).ToList();

            if (baseDrinkOptions.Count > 0) list.Add(baseDrinkOptions[UnityEngine.Random.Range(0, baseDrinkOptions.Count)]);
            if (frutaOptions.Count > 0) list.Add(frutaOptions[UnityEngine.Random.Range(0, frutaOptions.Count)]);
            if (acompanhamentoOptions.Count > 0) list.Add(acompanhamentoOptions[UnityEngine.Random.Range(0, acompanhamentoOptions.Count)]);
        }
    }

    private bool IsValid(Order order)
    {
        if (order == null || order.ingredientes == null || order.ingredientes.Count == 0) return false;

        bool containsOnlyFruits = order.ingredientes.All(i => i is Food f && f.category == FoodCategory.Fruta);
        if (containsOnlyFruits) return false;

        int friesCount = order.ingredientes.Count(i => i.ingredientName.Trim() == "Fries");
        if (friesCount > 1) return false;

        bool isFoodOrder = order.ingredientes.Any(i => i is Food);
        if (isFoodOrder)
        {
            foreach (Ingredient ingredient in order.ingredientes)
            {
                if (ingredient is Food f && f.category == FoodCategory.Fruta && f.type.HasFlag(IngredientFlags.Acompanhamento))
                {
                    return false;
                }
            }
        }

        return true;
    }

    private Order CreateDefaultOrder(int quantity)
    {
        List<Ingredient> allIngredients = foodIngredients.Cast<Ingredient>().Concat(beverageIngredients.Cast<Ingredient>()).ToList();

        List<Ingredient> ingredients = new List<Ingredient>();
        Ingredient principal = allIngredients.FirstOrDefault(i => i != null && i.type.HasFlag(IngredientFlags.Principal));
        Ingredient acompanhante = allIngredients.FirstOrDefault(i => i != null && i.type.HasFlag(IngredientFlags.Acompanhamento));

        if (principal != null) ingredients.Add(principal);
        if (acompanhante != null) ingredients.Add(acompanhante);

        if (ingredients.Count == 0 && allIngredients.Count > 0)
        {
            ingredients.Add(allIngredients[0]);
        }

        return new Order(quantity, ingredients);
    }
}