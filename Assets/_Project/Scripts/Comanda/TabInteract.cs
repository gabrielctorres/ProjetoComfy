using System;
using System.Collections.Generic; // Necessário para usar List<>
using UnityEngine;
using UnityEngine.UI; // Necessário para usar o componente Image
using TMPro;

public class TabInteract : MonoBehaviour, IInteractable
{

    public Image tabImage;

    [Header("Sprites")]
    public List<Sprite> availableSprites;

    [Header("Tab Settings")]
    public Tab fakeTab;
    public Tab originalTab;
    public int count;

    public static event Action<Tab, Tab> OnTabOpen;

    public void Start()
    {
        SetRandomSprite();
    }

    public void Interact()
    {
        OnTabOpen?.Invoke(this.fakeTab, this.originalTab);
    }

    private void SetRandomSprite()
    {
        if (tabImage == null)
        {
            Debug.LogWarning("O componente 'tabImage' não foi arrastado no Inspetor!");
            return;
        }

        if (availableSprites == null || availableSprites.Count == 0)
        {
            Debug.LogWarning("A lista 'availableSprites' está vazia!");
            return;
        }

        int randomIndex = UnityEngine.Random.Range(0, availableSprites.Count);

        tabImage.sprite = availableSprites[randomIndex];
    }
}