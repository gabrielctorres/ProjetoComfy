using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Ingredient", order = 1)]
public class Ingredient : ScriptableObject
{
    public string ingredientName;
    public TypeIngredient type;
}
public enum TypeIngredient
{
    Carne,
    Veggie,
}