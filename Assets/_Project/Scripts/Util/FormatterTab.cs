using System.Collections.Generic;
using System.Text;
using UnityEngine;

public static class FormatterTab
{
    public static string FormatPedido(Order pedido)
    {
        StringBuilder sb = new StringBuilder();
        string txtQuantity = $"<link=quantity><u>{pedido.quantity}</u></link>";
        sb.Append(txtQuantity + "x ");
        for (int i = 0; i < pedido.ingredientes.Count; i++)
        {
            if (pedido.ingredientes[i] == null) continue;

            string linkID = $"ingrediente_{i}";
            string linkText = pedido.ingredientes[i].ingredientName;
            sb.Append($"<link={linkID}><u>{linkText}</u></link>");
            if (i < pedido.ingredientes.Count - 1)
            {
                sb.Append(" c/ ");
            }
        }

        return sb.ToString();
    }

    public static string Mark(string linkID, string linkText)
    {
        string textModified = $"<link={linkID}><mark=#eb3434>{linkText}</mark></link>";

        return textModified;
    }
}
