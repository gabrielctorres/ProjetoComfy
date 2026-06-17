using System;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class IngredientsTab : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    List<Ingredient> allIngredients;
    public GameObject ingredientPrefab;
    public GameObject ingredientDrag;
    void Start()
    {
        allIngredients = new List<Ingredient>(Resources.LoadAll<Ingredient>("Dados/Ingredientes"));

        for (int i = 0; i <  allIngredients.Count; i++)
        {
            GameObject _prefab = Instantiate(ingredientPrefab, transform);
            _prefab.GetComponent<IngredientButton>().Init(allIngredients[i]);
        }

        gameObject.SetActive(false);
    }

}
