using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FactoryTab : MonoBehaviour
{
    public List<Food> foodIngredients;
    public List<Beverage> beverageIngredients;

    [Header("Configurações Padrão de Spawn")]
    [SerializeField] private int minOrders = 1;
    [SerializeField] private int maxOrders = 4;
    [SerializeField] private int maxQuantity = 3;

    private TabRules defaultRules;
    public TabRules DefaultRules => defaultRules;

    private void Awake()
    {
        defaultRules = new TabRules(minOrders, maxOrders, maxQuantity);
    }

    public Tab CreateTab(TabRules customRules = null)
    {
        var rules = customRules ?? defaultRules;
        var tab = ScriptableObject.CreateInstance<Tab>();
        tab.pedidos = new List<Order>();

        int total = UnityEngine.Random.Range(rules.minOrders, rules.maxOrders + 1);
        float roll = UnityEngine.Random.value;

        for (int i = 0; i < total; i++)
        {
            bool forceFood = roll < rules.foodOnlyChance || (roll >= rules.foodOnlyChance + rules.drinkOnlyChance && UnityEngine.Random.value > 0.5f);
            bool forceDrink = !forceFood;

            tab.pedidos.Add(GenerateSingleOrder(rules, forceFood, forceDrink));
        }
        tab.name = "Comanda Normal";
        return tab;
    }

    private Order GenerateSingleOrder(TabRules rules, bool forceFood, bool forceDrink)
    {
        bool isVegan = UnityEngine.Random.value < rules.veganChance;

        for (int attempt = 0; attempt < 10; attempt++)
        {
            var order = GenerateOrderAttempt(rules, forceFood, forceDrink, isVegan);
            if (IsValid(order)) return order;
        }

        Debug.Log($"[FactoryTab] O gerador falhou em criar um pedido válido após 10 tentativas! Gerando pedido padrão baseado nas regras. ForceFood: {forceFood}, ForceDrink: {forceDrink}");
        return CreateDefaultOrder(rules, UnityEngine.Random.Range(1, rules.maxQuantity + 1), forceFood, forceDrink, isVegan);
    }

    private Order GenerateOrderAttempt(TabRules rules, bool forceFood, bool forceDrink, bool isVegan)
    {
        var ingredients = new List<Ingredient>();

        if (forceFood) BuildFoodLine(ingredients, isVegan, rules);
        else if (forceDrink) BuildDrinkLine(ingredients, isVegan, rules);

        int qty = UnityEngine.Random.Range(1, rules.maxQuantity + 1);

        return ingredients.Count > 0 ? new Order(qty, ingredients) : CreateDefaultOrder(rules, qty, forceFood, forceDrink, isVegan);
    }

    private void BuildFoodLine(List<Ingredient> list, bool isVegan, TabRules rules)
    {
        var categories = GetFoodCategoriesExceptFruta();
        if (categories.Count == 0) return;

        var cat = categories[UnityEngine.Random.Range(0, categories.Count)];

        var principal = GetFoodWithVeganFallback(
            f => f.category == cat && f.type.HasFlag(IngredientFlags.Principal), isVegan);

        if (principal != null) list.Add(principal);

        bool skipSide = UnityEngine.Random.value < rules.sidelessChance;

        if (!skipSide)
        {
            var sideCategory = rules.varySideCategory
                ? categories[UnityEngine.Random.Range(0, categories.Count)]
                : cat;

            var acompanhamento = GetFoodWithVeganFallback(
                f => f.type.HasFlag(IngredientFlags.Acompanhamento) && f.category != FoodCategory.Fruta
                     && (!rules.varySideCategory || f.category == sideCategory),
                isVegan);

            if (acompanhamento == null && rules.varySideCategory)
            {
                acompanhamento = GetFoodWithVeganFallback(
                    f => f.type.HasFlag(IngredientFlags.Acompanhamento) && f.category != FoodCategory.Fruta,
                    isVegan);
            }

            if (acompanhamento != null) list.Add(acompanhamento);

            if (acompanhamento != null && UnityEngine.Random.value < rules.extraSideChance)
            {
                var segundo = GetFoodWithVeganFallback(
                    f => f.type.HasFlag(IngredientFlags.Acompanhamento) && f.category != FoodCategory.Fruta
                         && f != acompanhamento,
                    isVegan);

                if (segundo != null) list.Add(segundo);
            }
        }
    }

    private void BuildDrinkLine(List<Ingredient> list, bool isVegan, TabRules rules)
    {
        var targetCategory = UnityEngine.Random.value > 0.5f ? BeverageCategory.Drink : BeverageCategory.Juice;

        var baseDrink = GetBeverageWithVeganFallback(
            b => b.category == targetCategory && b.type.HasFlag(IngredientFlags.Principal), isVegan);

        if (baseDrink != null) list.Add(baseDrink);

        bool skipExtra = UnityEngine.Random.value < rules.sidelessChance;

        if (!skipExtra)
        {
            var fruta = GetFoodWithVeganFallback(
                f => f.category == FoodCategory.Fruta, isVegan);

            if (fruta != null) list.Add(fruta);

            if (fruta != null && UnityEngine.Random.value < rules.extraSideChance)
            {
                var segundaFruta = GetFoodWithVeganFallback(
                    f => f.category == FoodCategory.Fruta && f != fruta, isVegan);

                if (segundaFruta != null) list.Add(segundaFruta);
            }
        }
    }

    private bool IsValid(Order order)
    {
        if (order?.ingredientes == null || order.ingredientes.Count == 0) return false;
        if (order.ingredientes.All(IsFruit)) return false;
        if (order.ingredientes.Count(i => i.ingredientName.Trim() == "Fries") > 1) return false;

        bool hasBeverage = order.ingredientes.Any(i => i is Beverage);
        bool hasFood = order.ingredientes.Any(i => i is Food f && !IsFruit(f));

        if (hasBeverage && hasFood) return false;
        if (hasFood && order.ingredientes.Any(IsFruit)) return false;

        return true;
    }

    private bool IsFruit(Ingredient i)
    {
        return i is Food f && f.category == FoodCategory.Fruta;
    }

    private Food GetFoodWithVeganFallback(Func<Food, bool> predicate, bool isVegan)
    {
        var filtered = FilterFood(predicate, isVegan);
        if (filtered.Count == 0 && !isVegan)
        {
            filtered = FilterFood(predicate, false);
        }

        if (filtered.Count > 0)
        {
            return filtered[UnityEngine.Random.Range(0, filtered.Count)];
        }
        return null;
    }

    private Beverage GetBeverageWithVeganFallback(Func<Beverage, bool> predicate, bool isVegan)
    {
        var filtered = FilterBeverage(predicate, isVegan);
        if (filtered.Count == 0 && !isVegan)
        {
            filtered = FilterBeverage(predicate, false);
        }

        if (filtered.Count > 0)
        {
            return filtered[UnityEngine.Random.Range(0, filtered.Count)];
        }
        return null;
    }

    private List<Food> FilterFood(Func<Food, bool> predicate, bool veganOnly)
    {
        var result = new List<Food>();
        foreach (var item in foodIngredients)
        {
            if (item != null && predicate(item) && (!veganOnly || item.isVegan))
            {
                result.Add(item);
            }
        }
        return result;
    }

    private List<Beverage> FilterBeverage(Func<Beverage, bool> predicate, bool veganOnly)
    {
        var result = new List<Beverage>();
        foreach (var item in beverageIngredients)
        {
            if (item != null && predicate(item) && (!veganOnly || item.isVegan))
            {
                result.Add(item);
            }
        }
        return result;
    }

    private List<FoodCategory> GetFoodCategoriesExceptFruta()
    {
        var result = new List<FoodCategory>();
        foreach (FoodCategory v in Enum.GetValues(typeof(FoodCategory)))
        {
            if (v != FoodCategory.Fruta)
            {
                result.Add(v);
            }
        }
        return result;
    }

    private Order CreateDefaultOrder(TabRules rules, int quantity, bool forceFood, bool forceDrink, bool isVegan)
    {
        var ingredients = new List<Ingredient>();

        if (forceDrink)
        {
            var defaultDrink = GetBeverageWithVeganFallback(b => b.type.HasFlag(IngredientFlags.Principal), isVegan);
            if (defaultDrink != null) ingredients.Add(defaultDrink);
        }
        else
        {
            var defaultFood = GetFoodWithVeganFallback(f => f.type.HasFlag(IngredientFlags.Principal) && f.category != FoodCategory.Fruta, isVegan);
            if (defaultFood != null) ingredients.Add(defaultFood);
        }

        if (ingredients.Count == 0)
        {
            if (foodIngredients.Count > 0) ingredients.Add(foodIngredients[0]);
            else if (beverageIngredients.Count > 0) ingredients.Add(beverageIngredients[0]);
        }

        return new Order(quantity, ingredients);
    }
}