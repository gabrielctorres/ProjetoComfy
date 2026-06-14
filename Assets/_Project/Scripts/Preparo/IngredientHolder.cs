using NUnit.Framework;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.UI;


public class IngredientHolder : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    
    public IngredientCup ingredientCup;
    public Image img;


    private void OnEnable() { ingredientCup.RegisterListener(this); }
    private void OnDisable() { ingredientCup.UnregisterListener(this); }

    void Start()
    {
        ingredientCup.ClearIngredients();
        Transform filho = transform.Find("FillImage");
        if (filho != null)
        {
            img = filho.GetComponent<Image>();
        }
    }

    public void UpdateQuantity(int quantidade, int limite)
    {
        if (img == null) {
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
