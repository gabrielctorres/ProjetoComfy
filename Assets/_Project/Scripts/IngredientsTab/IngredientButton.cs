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
        IngredientContainer _ingHolderContainer;
        transform.parent.gameObject.SetActive(false);
        if (PreparoManager.activeWindow.TryGetComponent<PreparoTab>(out janela))
        {
            _ingredientHolder = janela.ingredientHolder.GetComponent<IngredientHolder>();
            _ingHolderContainer = _ingredientHolder.ingredientContainer;
            Debug.Log(_ingHolderContainer);
            if (janela.busy || _ingredientHolder == null || _ingHolderContainer.ingredients.Count >= _ingHolderContainer.maxIngredients) { return; }
        } else {
            //Debug.Log("ué deu trygetcomponent como false?");
            return;
        }

        janela.busy = true;

        PreparoSystem.Instance.StartCoroutine(WaitAndAddIngredient(janela, _ingHolderContainer));

    }

    IEnumerator WaitAndAddIngredient(PreparoTab janela, IngredientContainer ingredientContainer)
    {
        yield return new WaitForSeconds(janela.prepareTime);
        ingredientContainer.AddIngredient(ingrediente);
        janela.busy = false;
    }


}
