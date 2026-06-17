using System.Collections;
using TMPro;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class IngredientButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    Ingredient ingrediente;
    PreparoSystem PreparoManager;

    float isHolding = -1f;
    void Start()
    {
        PreparoManager = GetComponentInParent<PreparoSystem>();
    }

    public void Init(Ingredient _ing)
    {
        //Debug.Log("Init chamado em: " + gameObject.name + " | ID: " + GetEntityId());
        ingrediente = _ing;
        TextMeshProUGUI texto = GetComponentInChildren<TextMeshProUGUI>();
        texto.text = ingrediente.name;
    }

    public void OnClick()
    {
        /*
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
        */

    }

    void Update()
    {
        if (isHolding < 0) { return; }
        //Debug.Log("Holding... ");
        if (Mouse.current.leftButton.wasReleasedThisFrame) { isHolding = -1; return; }
        isHolding += Time.deltaTime;
        if (isHolding > 0.2)
        {
            isHolding = -1;
            GameObject ingredientDragOBJ = GetComponentInParent<IngredientsTab>().ingredientDrag;
            IngredientDrag ingredientDrag = ingredientDragOBJ.GetComponent<IngredientDrag>();
            /*Debug.Log("Drag OBJ: " + ingredientDragOBJ.name);
            Debug.Log("Instance ID: " + ingredientDrag.GetEntityId());
            Debug.Log("Clique em: " + gameObject.name + " | ID: " + GetEntityId());
            Debug.Log("Drag ingredient: " + ingrediente.name); -- Debug pra ver oq tava errado, era o ingredient null pq o script tava no text do prefab tb*/
            ingredientDrag.StartDragging(ingrediente);
            transform.parent.gameObject.SetActive(false);
        }
    }

    IEnumerator WaitAndAddIngredient(PreparoTab janela, IngredientContainer ingredientContainer)
    {
        yield return new WaitForSeconds(janela.prepareTime);
        ingredientContainer.AddIngredient(ingrediente);
        janela.busy = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (isHolding < 0)
        { isHolding = 0; }
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        isHolding = -1;
    }
}
