using System;
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

    public static event Action<List<string>> OnDoubtSubmitted;

    private bool isLocked = false;

    public void OnEnable()
    {
        LinkHandlerForTMPText.OnClickedOnLinkEvent += AddDoubt;
        WaiterSpawner.OnWaiterSpawned += ResetDoubtSystem;
    }

    public void OnDisable()
    {
        LinkHandlerForTMPText.OnClickedOnLinkEvent -= AddDoubt;
        WaiterSpawner.OnWaiterSpawned -= ResetDoubtSystem;
    }

    void Update()
    {
        doubtText.text = $"{doubts.Count}/{douptMax}";
    }

    public void AddDoubt(string id, string linkText)
    {
        if (isLocked || doubts.Count >= douptMax) return;

        if (!doubts.Contains(id))
        {
            doubts.Add(id);
            ApplyRichStyle(id, "mark=#eb3434");
        }
    }

    public void SubmitDoubts()
    {
        if (isLocked || doubts.Count == 0) return;

        isLocked = true;
        OnDoubtSubmitted?.Invoke(new List<string>(doubts));
    }

    public void ResetDoubtSystem(Waiter newWaiter)
    {
        doubts.Clear();
        isLocked = false;
    }

    public void ApplyRichStyle(string linkID, string innerStyle)
    {
        string textoOriginal = tabText.text;

        string pattern = @"<link=""" + Regex.Escape(linkID) + @""">(.*?)</link>";

        if (Regex.IsMatch(textoOriginal, pattern))
        {
            tabText.text = Regex.Replace(textoOriginal, pattern, match =>
            {
                string textoInternoReal = match.Groups[1].Value;
                string limpo = Regex.Replace(textoInternoReal, @"<[^>]*>", "");

                return $"<link=\"{linkID}\"><{innerStyle}>{limpo}</{innerStyle}></link>";
            });
        }
    }
}