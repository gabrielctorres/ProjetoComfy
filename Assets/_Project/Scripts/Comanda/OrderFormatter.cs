using System.Text;
using UnityEngine;

public class OrderFormatter : MonoBehaviour
{
    public string FormatPedido(Order pedido, int orderIndex)
    {
        StringBuilder sb = new StringBuilder();

        string quantityLinkID = $"quantity_{orderIndex}";
        string txtQuantity = $"<link=\"{quantityLinkID}\"><u>{pedido.quantity}</u></link>";
        sb.Append(txtQuantity + "x ");

        for (int i = 0; i < pedido.ingredientes.Count; i++)
        {
            if (pedido.ingredientes[i] == null) continue;

            string linkID = $"ingrediente_{orderIndex}_{i}";
            string linkText = pedido.ingredientes[i].ingredientName;

            sb.Append($"<link=\"{linkID}\"><u>{linkText}</u></link>");

            if (i < pedido.ingredientes.Count - 1)
            {
                sb.Append(" c/ ");
            }
        }

        return sb.ToString();
    }
}

