using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public abstract class IngredientContainer : ScriptableObject
{
    public List<Ingredient> ingredients;
    public int maxIngredients;

    public abstract void AddIngredient(Ingredient ingredient);

    public abstract void RegisterListener(IngredientHolder listener);

    public abstract void UnregisterListener(IngredientHolder listener);

    public abstract void ClearIngredients();
}
