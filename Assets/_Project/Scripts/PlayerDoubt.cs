using System.Collections.Generic;
using TMPro;
using UnityEngine;

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
            ApplyRich(id, linkText);
        }
        else
        {
            Debug.Log("Doubt limit reached. Cannot add more doubts.");
        }
    }

    public void ApplyRich(string linkID, string linkText)
    {
        string texOriginal = tabText.text;
        string formatedOrigin = $"<link={linkID}><u>{linkText}</u></link>";
        string textModified = FormatterTab.Mark(linkID, linkText);

        if (texOriginal.Contains(formatedOrigin))
        {
            tabText.text = texOriginal.Replace(formatedOrigin, textModified);
        }
    }
}
