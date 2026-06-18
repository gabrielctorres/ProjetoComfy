using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCup", menuName = "ScriptableObjects/IngredientCup")]
public class IngredientCup : IngredientContainer
{
    // Mudado para aceitar qualquer Listener que implemente a interface
    public List<IIngredientListener> listeners = new List<IIngredientListener>();

    void Signal()
    {
        for (int i = 0; i < listeners.Count; i++)
        {
            listeners[i].OnIngredientsChanged(ingredients, maxIngredients);
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

    public override void ClearIngredients()
    {
        ingredients.Clear();
        Signal();
    }

    public override void RegisterListener(IIngredientListener listener) { listeners.Add(listener); }
    public override void UnregisterListener(IIngredientListener listener) { listeners.Remove(listener); }
}