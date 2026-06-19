using System;
using UnityEngine;

[Flags]
public enum IngredientFlags
{
    None = 0,
    Principal = 1 << 0,
    Acompanhamento = 1 << 1
}

public abstract class Ingredient : ScriptableObject
{
    public string ingredientName;
    public Sprite icon;
    public Sprite dragIcon;
    public IngredientFlags type;
    public bool isVegan;
}
