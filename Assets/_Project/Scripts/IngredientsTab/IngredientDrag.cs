using UnityEngine;
using UnityEngine.InputSystem;

public class IngredientDrag : MonoBehaviour
{

    public Ingredient ingrediente;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Mouse.current == null) { return; } //early return pra se não conseguir pegar input do mouse.

        Vector3 mousePos = Mouse.current.position.ReadValue();

        if (Mouse.current.leftButton.wasReleasedThisFrame) //código executado ao soltar o clique do mouse
        {
            CheckLocation(mousePos);
            //Debug.Log(ingrediente.ToString());
            gameObject.SetActive(false);
            ingrediente = null;
            return;
        }

        transform.position = mousePos;
    }

    void CheckLocation(Vector3 mousePos) //código para checar se foi solto em algum lugar tipo o espremedor, coqueteleira ou panela
    {
        
    }

    public void StartDragging(Ingredient novoIngrediente) //Código de inicialização do objeto arrastado.
    {
        /*Debug.Log("Ingrediente do drag: " + novoIngrediente.name);
        Debug.Log("Meu ID: " + GetEntityId());*/
        ingrediente = novoIngrediente;
        transform.position = Mouse.current.position.ReadValue();
        gameObject.SetActive(true);
    }

}
