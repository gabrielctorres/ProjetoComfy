using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewRecipe", menuName = "ScriptableObjects/Recipe")]
public class DrinkRecipe : ScriptableObject
{
    public string recipeName;
    public List<Ingredient> requiredIngredients;
    public Sprite resultSprite;

    public bool Matches(List<Ingredient> ingredientsInCup)
    {
        if (ingredientsInCup.Count != requiredIngredients.Count) return false;

        List<Ingredient> checkList = new List<Ingredient>(ingredientsInCup);

        foreach (var reqIng in requiredIngredients)
        {
            if (checkList.Contains(reqIng))
            {
                checkList.Remove(reqIng);
            }
            else
            {
                return false;
            }
        }

        return true;
    }
}