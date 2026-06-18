using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngredientHolder : MonoBehaviour, IIngredientListener
{
    public IngredientContainer ingredientContainer;
    public Image img;

    private void OnEnable()
    {
        ingredientContainer.RegisterListener(this);
        // Garante a atualização inicial ao ativar o objeto
        OnIngredientsChanged(ingredientContainer.ingredients, ingredientContainer.maxIngredients);
    }

    private void OnDisable()
    {
        ingredientContainer.UnregisterListener(this);
    }

    void Start()
    {
        ingredientContainer.ClearIngredients();
        FindFillImage();
    }

    // O prato chama essa função da interface, que atualiza a sua barra de fillAmount
    public void OnIngredientsChanged(List<Ingredient> ingredients, int maxIngredients)
    {
        UpdateQuantity(ingredients.Count, maxIngredients);
    }

    public void UpdateQuantity(int quantidade, int limite)
    {
        if (img == null)
        {
            FindFillImage();
        }

        if (img != null && limite > 0)
        {
            img.fillAmount = (float)quantidade / limite;
        }
    }

    private void FindFillImage()
    {
        Transform filho = transform.Find("FillImage");
        if (filho != null)
        {
            img = filho.GetComponent<Image>();
        }
    }
}