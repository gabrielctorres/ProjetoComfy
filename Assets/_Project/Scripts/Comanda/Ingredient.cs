using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Ingredient", order = 1)]
public class Ingredient : ScriptableObject
{
    public string ingredientName;
    public IngredientFlags type;
}

[System.Flags]
public enum IngredientFlags
{
    None = 0,
    Comida = 1 << 0,
    Bebida = 1 << 1,
    Vegano = 1 << 2,
    Principal = 1 << 3,
    Secundario = 1 << 4,
    Acompanhamento = 1 << 5
}