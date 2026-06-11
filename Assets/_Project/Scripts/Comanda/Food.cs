using UnityEngine;

public enum FoodCategory
{
    Sanduiche,
    Porcao,
    Fruta
}

[CreateAssetMenu(fileName = "NewFood", menuName = "ScriptableObjects/Food")]
public class Food : Ingredient
{
    public FoodCategory category;
}
