using System.Collections.Generic;
using UnityEngine;

public class DrinkDisplayer : MonoBehaviour, IIngredientListener
{
    public IngredientContainer ingredientContainer;
    public List<DrinkRecipe> allRecipes;
    public Sprite defaultSprite;

    private SpriteRenderer drinkSpriteRenderer;

    private void Awake()
    {
        drinkSpriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        if (ingredientContainer != null)
        {
            ingredientContainer.RegisterListener(this);
            OnIngredientsChanged(ingredientContainer.ingredients, ingredientContainer.maxIngredients);
        }
    }

    private void OnDisable()
    {
        if (ingredientContainer != null)
        {
            ingredientContainer.UnregisterListener(this);
        }
    }

    public void OnIngredientsChanged(List<Ingredient> ingredients, int maxIngredients)
    {
        if (drinkSpriteRenderer == null) return;

        if (ingredients == null || ingredients.Count == 0)
        {
            drinkSpriteRenderer.sprite = defaultSprite;
            return;
        }



        foreach (var recipe in allRecipes)
        {
            if (recipe != null && recipe.Matches(ingredients))
            {
                drinkSpriteRenderer.sprite = recipe.resultSprite;
                return;
            }
        }

        drinkSpriteRenderer.sprite = defaultSprite;
    }
}