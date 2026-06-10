using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Text.RegularExpressions;

public class PlayerDoubt : MonoBehaviour
{
    public int douptMax = 3;
    public List<string> doubts = new List<string>();

    public TextMeshProUGUI doubtText;
    public TMP_Text tabText;
    public void OnEnable()
    {
        LinkHandlerForTMPText.OnClickedOnLinkEvent += AddDoubt;
    }
    public void OnDisable()
    {
        LinkHandlerForTMPText.OnClickedOnLinkEvent -= AddDoubt;
    }
    void Update()
    {
        doubtText.text = $"{doubts.Count}/{douptMax}";
    }

    public void AddDoubt(string id, string linkText)
    {
        if (doubts.Count < douptMax)
        {
            doubts.Add(linkText);
            ApplyRichStyle(id, "mark=#eb3434");
        }
    }

    public void ApplyRichStyle(string linkID, string innerStyle)
    {
        string textoOriginal = tabText.text;

        string pattern = @"<link=""" + linkID + @""">(.*?)</link>";

        if (Regex.IsMatch(textoOriginal, pattern))
        {
            tabText.text = Regex.Replace(textoOriginal, pattern, match =>
            {
                string textoInternoReal = match.Groups[1].Value;

                return $"<link=\"{linkID}\"><{innerStyle}>{textoInternoReal}</{innerStyle}></link>";
            });
        }
    }
}
