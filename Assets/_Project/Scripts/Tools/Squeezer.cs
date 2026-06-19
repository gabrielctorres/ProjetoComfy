using System.Collections;
using UnityEngine;

public class Squeezer : Tool
{
    public IngredientContainer targetContainer;
    public float timeToSqueeze = 2f;
    private bool isBusy = false;

    protected override void OnMouseEnter()
    {
        if (isBusy || !DragManager.Instance.IsDragging()) return;
        if (targetContainer.ingredients.Count >= targetContainer.maxIngredients) return;

        Ingredient currentIngredient = DragManager.Instance.CurrentIngredient;

        if (IsValidForSqueezer(currentIngredient))
        {
            Execute(currentIngredient);
        }
        else
        {
            Debug.Log($"O Squeezer não aceita o ingrediente: {currentIngredient.ingredientName}");
        }
    }

    private bool IsValidForSqueezer(Ingredient ingredient)
    {
        if (ingredient is Beverage beverage && beverage.category == BeverageCategory.Juice)
        {
            return true;
        }

        if (ingredient is Food food && food.category == FoodCategory.Fruta)
        {
            return true;
        }

        return false;
    }

    protected override void Execute(Ingredient ingredient)
    {
        Debug.Log($"Squeezer consumiu: {ingredient.ingredientName}. Espremendo...");

        DragManager.Instance.ClearDraggedItem();

        StartCoroutine(SqueezeRoutine(ingredient));
    }

    private IEnumerator SqueezeRoutine(Ingredient ingredientAdicionado)
    {
        isBusy = true;

        yield return new WaitForSeconds(timeToSqueeze);

        if (targetContainer != null)
        {
            targetContainer.AddIngredient(ingredientAdicionado);
            Debug.Log("Processo concluído e adicionado ao container!");
        }

        isBusy = false;
    }
}