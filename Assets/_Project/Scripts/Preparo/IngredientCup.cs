using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewCup", menuName = "ScriptableObjects/IngredientCup")]
public class IngredientCup : IngredientContainer
{

    public List<IngredientHolder> listeners;

    void Signal()
    {
        for (int i = 0; i < listeners.Count; i++)
        {
            listeners[i].UpdateQuantity(ingredients.Count, maxIngredients);
        }
    }

    public override void AddIngredient(Ingredient _ing)
    {
        if (!ingredients.Contains(_ing))
        { 
            ingredients.Add(_ing);
            Signal();
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void ClearIngredients()
    {
        ingredients.Clear();
        Signal();
    }

    public override void RegisterListener(IngredientHolder listener) { listeners.Add(listener); }
    public override void UnregisterListener(IngredientHolder listener) { listeners.Remove(listener); }

}
