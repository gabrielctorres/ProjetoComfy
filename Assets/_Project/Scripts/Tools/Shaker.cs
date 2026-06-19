using System.Collections;
using UnityEngine;

public class Shaker : Tool
{
    public IngredientContainer targetContainer;
    public float timeToShake = 2f;
    private bool isBusy = false;

    protected override void OnMouseEnter()
    {
        if (isBusy || !DragManager.Instance.IsDragging()) return;
        if (targetContainer.ingredients.Count >= targetContainer.maxIngredients) return;

        Ingredient currentIngredient = DragManager.Instance.CurrentIngredient;

        if (IsValidForShaker(currentIngredient))
        {
            Execute(currentIngredient);
        }
        else
        {
            Debug.Log($"O Shaker não aceita o ingrediente: {currentIngredient.ingredientName}");
        }
    }

    private bool IsValidForShaker(Ingredient ingredient)
    {
        if (ingredient is Beverage beverage && beverage.category == BeverageCategory.Drink)
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
        Debug.Log($"Shaker consumiu: {ingredient.ingredientName}. Misturando...");

        DragManager.Instance.ClearDraggedItem();

        StartCoroutine(ShakeRoutine(ingredient));
    }

    private IEnumerator ShakeRoutine(Ingredient ingredientAdicionado)
    {
        isBusy = true;

        yield return new WaitForSeconds(timeToShake);

        if (targetContainer != null)
        {
            targetContainer.AddIngredient(ingredientAdicionado);
            Debug.Log("Mistura concluída e adicionada ao container!");
        }

        isBusy = false;
    }
}