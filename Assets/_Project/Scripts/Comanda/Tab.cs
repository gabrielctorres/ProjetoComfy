using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/TabData", order = 1)]
public class Tab : ScriptableObject
{
    public List<Order> pedidos;
}

[System.Serializable]
public class Order
{
    public int quantity;
    public List<Ingredient> ingredientes;

    public Order(int quantity, List<Ingredient> ingredientes)
    {
        this.quantity = quantity;
        this.ingredientes = ingredientes;
    }
}