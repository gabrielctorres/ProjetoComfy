using System.Collections;
using TMPro;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class IngredientButton : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    Ingredient ingrediente;
    PreparoSystem PreparoManager;
    void Start()
    {
        PreparoManager = GetComponentInParent<PreparoSystem>();
    }

    public void Init(Ingredient _ing)
    {
        ingrediente = _ing;
        TextMeshProUGUI texto = GetComponentInChildren<TextMeshProUGUI>();
        texto.text = ingrediente.name;
    }

    public void OnClick()
    {
        if (ingrediente == null) { return; }
        if (!PreparoManager.activeWindow) { return; }
        PreparoTab janela;
        IngredientHolder _ingredientHolder;
        transform.parent.gameObject.SetActive(false);
        if (PreparoManager.activeWindow.TryGetComponent<PreparoTab>(out janela))
        {
            _ingredientHolder = janela.ingredientHolder.GetComponent<IngredientHolder>();
            if (janela.busy || _ingredientHolder == null || _ingredientHolder.ingredientCup.ingredients.Count >= _ingredientHolder.ingredientCup.maxIngredients) { return;  }
        } else {
            //Debug.Log("ué deu trygetcomponent como false?");
            return;
        }

        janela.busy = true;

        PreparoSystem.Instance.StartCoroutine(WaitAndAddIngredient(janela, _ingredientHolder));

    }

    IEnumerator WaitAndAddIngredient(PreparoTab janela, IngredientHolder ingredientHolder)
    {
        yield return new WaitForSeconds(janela.prepareTime);
        ingredientHolder.ingredientCup.AddIngredient(ingrediente);
        janela.busy = false;
    }


}
