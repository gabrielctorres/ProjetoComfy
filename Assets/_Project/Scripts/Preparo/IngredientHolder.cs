using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class IngredientHolder : MonoBehaviour, IIngredientListener
{
    public IngredientContainer ingredientContainer;

    [Header("Configurações do Efeito Visual")]
    public float animationDuration = 0.25f;
    public float punchIntensity = 0.12f;

    private Vector3 originalScale;

    private void Awake()
    {
        originalScale = transform.localScale;
    }

    private void OnEnable()
    {
        ingredientContainer.RegisterListener(this);
        OnIngredientsChanged(ingredientContainer.ingredients, ingredientContainer.maxIngredients);
    }

    private void OnDisable()
    {
        ingredientContainer.UnregisterListener(this);
    }

    void Start()
    {
        ingredientContainer.ClearIngredients();
    }

    public void OnIngredientsChanged(List<Ingredient> ingredients, int maxIngredients)
    {

        if (ingredients != null && ingredients.Count > 0)
        {

            transform.DOKill();
            transform.localScale = originalScale;

            transform.DOPunchScale(new Vector3(punchIntensity, punchIntensity, 0f), animationDuration, vibrato: 5, elasticity: 1f).SetEase(Ease.OutQuad);
        }
    }
}