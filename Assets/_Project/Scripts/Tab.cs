using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/TabData", order = 1)]
public class Tab : ScriptableObject
{
    public List<Pedido> pedidos;
}

[System.Serializable]
public class Pedido
{
    public int quantity;
    public List<Ingredient> ingredientes;

    public Pedido(int quantity, List<Ingredient> ingredientes)
    {
        this.quantity = quantity;
        this.ingredientes = ingredientes;
    }
}