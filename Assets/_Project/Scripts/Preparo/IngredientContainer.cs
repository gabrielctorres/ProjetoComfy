using System.Collections.Generic;
using UnityEngine;

public interface IIngredientListener
{
    void OnIngredientsChanged(List<Ingredient> ingredients, int maxIngredients);
}
public abstract class IngredientContainer : ScriptableObject
{
    public List<Ingredient> ingredients;
    public int maxIngredients;

    public abstract void AddIngredient(Ingredient ingredient);

    // Agora aceitam a interface genérica
    public abstract void RegisterListener(IIngredientListener listener);
    public abstract void UnregisterListener(IIngredientListener listener);

    public abstract void ClearIngredients();
}