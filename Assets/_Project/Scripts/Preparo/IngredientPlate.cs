using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPlate", menuName = "ScriptableObjects/IngredientPlate")]
public class IngredientPlate : IngredientContainer
{
    // Agora aceita qualquer listener que use a interface (tanto a barra quanto o exibidor de sprite)
    public List<IIngredientListener> listeners = new List<IIngredientListener>();

    void Signal()
    {
        for (int i = 0; i < listeners.Count; i++)
        {
            // Passa a lista de ingredientes e o máximo exigido pela interface
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

    // Overrides usando a interface genérica
    public override void RegisterListener(IIngredientListener listener) { listeners.Add(listener); }
    public override void UnregisterListener(IIngredientListener listener) { listeners.Remove(listener); }
}