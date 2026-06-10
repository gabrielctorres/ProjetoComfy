using UnityEngine;

public enum BeverageCategory
{
    Juice,
    Drink
}

[CreateAssetMenu(fileName = "NewBeverage", menuName = "ScriptableObjects/Beverage")]
public class Beverage : Ingredient
{
    public BeverageCategory category;

    public int strength;
}
