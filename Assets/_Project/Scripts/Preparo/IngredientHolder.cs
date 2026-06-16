using NUnit.Framework;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.UI;


public class IngredientHolder : MonoBehaviour
{


    public IngredientContainer ingredientContainer;
    public Image img;




    private void OnEnable()
    {
        ingredientContainer.RegisterListener(this);
        UpdateQuantity(ingredientContainer.ingredients.Count, ingredientContainer.maxIngredients);

    }
    private void OnDisable()
    {
        ingredientContainer.UnregisterListener(this);
    }

    void Start()
    {
        ingredientContainer.ClearIngredients();
        Transform filho = transform.Find("FillImage");
        if (filho != null)
        {
            img = filho.GetComponent<Image>();
        }
    }

    public void UpdateQuantity(int quantidade, int limite)
    {
        if (img == null)
        {
            Transform filho = transform.Find("FillImage");
            if (filho != null)
            {
                img = filho.GetComponent<Image>();
            }

        }
        img.fillAmount = (float)quantidade / limite;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
