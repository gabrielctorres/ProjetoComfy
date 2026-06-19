using Unity.VisualScripting;
using UnityEngine;

public class PreparoTab : MonoBehaviour
{
    public bool busy = false;
    public GameObject ingredientHolder;
    public float prepareTime = 1f;
    void Start()
    {
        gameObject.SetActive(false);
        //if (!ingredientsHolder) Debug.Log("Paia, esqueceu do ingredientsHolder ai num script de Aba de preparo. (PreparoTab)");
    }
}
