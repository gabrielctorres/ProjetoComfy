using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class IngredientsNode : MonoBehaviour
{
    List<Ingredient> allIngredients;
    public GameObject ingredientPrefab;
    public Transform container;
    void Start()
    {
        allIngredients = new List<Ingredient>(Resources.LoadAll<Ingredient>("Dados/Ingredientes"));

        for (int i = 0; i < allIngredients.Count; i++)
        {
            GameObject _prefab = Instantiate(ingredientPrefab, container);
            _prefab.GetComponent<IngredientButton>().Init(allIngredients[i]);
            _prefab.GetComponent<UnityEngine.UI.Image>().sprite = allIngredients[i].icon;
        }

        gameObject.SetActive(false);
    }

}
