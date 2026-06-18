using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class DrinkDisplayer : MonoBehaviour, IIngredientListener // Também implementa a interface
{
    public IngredientContainer ingredientContainer;
    public List<DrinkRecipe> allRecipes; // Suas receitas em ScriptableObjects
    public Sprite defaultSprite; // Sprite do copo vazio / erro

    private Image drinkImage;

    private void Awake()
    {
        drinkImage = GetComponent<Image>();
    }

    private void OnEnable()
    {
        ingredientContainer.RegisterListener(this);
        OnIngredientsChanged(ingredientContainer.ingredients, ingredientContainer.maxIngredients);
    }

    private void OnDisable()
    {
        ingredientContainer.UnregisterListener(this);
    }

    // Quando o container muda, essa lógica roda de forma independente da barra de preenchimento
    public void OnIngredientsChanged(List<Ingredient> ingredients, int maxIngredients)
    {
        // Se estiver vazio, garante o sprite padrão
        if (ingredients == null || ingredients.Count == 0)
        {
            drinkImage.sprite = defaultSprite;
            return;
        }

        // Só tenta craftar/mudar sprite se atingir o máximo de ingredientes (ex: 3)
        if (ingredients.Count == maxIngredients)
        {
            foreach (var recipe in allRecipes)
            {
                if (recipe.Matches(ingredients))
                {
                    drinkImage.sprite = recipe.resultSprite;
                    return; // Achou a combinação, para o loop
                }
            }
        }

        // Se tem ingredientes mas não completou a receita ou deu uma combinação errada
        drinkImage.sprite = defaultSprite;
    }
}