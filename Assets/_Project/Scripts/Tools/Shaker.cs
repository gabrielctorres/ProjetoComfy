using System.Collections;
using UnityEngine;
using DG.Tweening; // Importado para usar o DOTween

public class Shaker : Tool
{
    public IngredientContainer targetContainer;
    public float timeToShake = 2f;
    private bool isBusy = false;

    [Header("Animação e Efeitos")]
    public float shakeStrength = 0.05f;
    public int shakeVibrato = 10;
    private Tween shakeTween;
    private Vector3 originalPosition;

    private void Awake()
    {
        originalPosition = transform.localPosition;
    }

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

        shakeTween = transform.DOShakePosition(timeToShake, strength: shakeStrength, vibrato: shakeVibrato, randomness: 90, snapping: false, fadeOut: false);

        yield return new WaitForSeconds(timeToShake);

        if (targetContainer != null)
        {
            targetContainer.AddIngredient(ingredientAdicionado);
            Debug.Log("Mistura concluída e adicionada ao container!");
        }

        StopShakeAnimation();

        isBusy = false;
    }

    private void StopShakeAnimation()
    {
        if (shakeTween != null && shakeTween.IsActive())
        {
            shakeTween.Kill();
        }
        transform.localPosition = originalPosition;
    }

    private void OnDisable()
    {
        StopShakeAnimation();
    }
}