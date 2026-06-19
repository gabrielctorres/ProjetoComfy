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
    private string originalFormattedText = "";

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

    public void SetNewTabText(string formattedText)
    {
        originalFormattedText = formattedText;
        tabText.text = formattedText;
    }

    public void AddDoubt(string id, string linkText)
    {
        if (isLocked) return;

        if (string.IsNullOrEmpty(originalFormattedText) && tabText != null)
        {
            originalFormattedText = tabText.text;
        }

        if (doubts.Contains(id))
        {
            doubts.Remove(id);
        }
        else
        {
            if (doubts.Count >= douptMax)
            {
                doubts.RemoveAt(0);
            }

            doubts.Add(id);
        }

        RenderDoubts();
    }

    private void RenderDoubts()
    {
        if (string.IsNullOrEmpty(originalFormattedText)) return;

        string textWithStyles = originalFormattedText;

        foreach (string id in doubts)
        {
            string pattern = @"<link=""" + Regex.Escape(id) + @""">(.*?)</link>";

            if (Regex.IsMatch(textWithStyles, pattern))
            {
                textWithStyles = Regex.Replace(textWithStyles, pattern, match =>
                {
                    string textoInterno = match.Groups[1].Value;
                    return $"<link=\"{id}\"><mark=#eb3434>{textoInterno}</mark></link>";
                });
            }
        }

        tabText.text = textWithStyles;
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

        if (!string.IsNullOrEmpty(originalFormattedText))
        {
            tabText.text = originalFormattedText;
        }
    }
}