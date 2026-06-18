using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DrinkRecipe", menuName = "ScriptableObjects/DrinkRecipe")]
public class DrinkRecipe : ScriptableObject
{
    public string recipeName;
    public Sprite resultSprite; // O sprite que você quer mostrar

    [Header("Combinação Necessária")]
    public Ingredient baseIngredient;
    public Ingredient fruitIngredient;
    public Ingredient sideIngredient; // Acompanhamento

    // Função que checa se a lista de ingredientes atual bate com essa receita
    public bool Matches(List<Ingredient> currentIngredients)
    {
        if (currentIngredients.Count != 3) return false;

        // Verifica se todos os ingredientes necessários estão na lista atual
        return currentIngredients.Contains(baseIngredient) &&
               currentIngredients.Contains(fruitIngredient) &&
               currentIngredients.Contains(sideIngredient);
    }
}