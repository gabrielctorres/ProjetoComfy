using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewCup", menuName = "ScriptableObjects/IngredientCup")]
public class IngredientCup : ScriptableObject
{

    public int maxIngredients = 3;
    public List<Ingredient> ingredients;

    public List<IngredientHolder> listeners;

    void Signal()
    {
        for (int i = 0; i < listeners.Count; i++)
        {
            listeners[i].UpdateQuantity(ingredients.Count, maxIngredients);
        }
    }

    public void AddIngredient(Ingredient _ing)
    {
        if (!ingredients.Contains(_ing))
        { 
            ingredients.Add(_ing);
            Signal();
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void ClearIngredients()
    {
        ingredients.Clear();
        Signal();
    }

    public void RegisterListener(IngredientHolder listener) { listeners.Add(listener); }
    public void UnregisterListener(IngredientHolder listener) { listeners.Remove(listener); }

}
