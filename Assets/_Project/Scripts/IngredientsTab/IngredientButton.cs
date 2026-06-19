using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class IngredientButton : MonoBehaviour
{
    public Ingredient ingrediente { get; private set; }
    private Image buttonImage;

    void Awake()
    {
        buttonImage = GetComponent<Image>();
    }

    public void Init(Ingredient _ing)
    {
        ingrediente = _ing;
        TextMeshProUGUI texto = GetComponentInChildren<TextMeshProUGUI>(true);
        if (texto != null) texto.text = ingrediente.name;
    }

    public void OnClick()
    {
        if (ingrediente == null) return;

        DragManager.Instance.SetDraggedItem(ingrediente, buttonImage.sprite);
    }
}