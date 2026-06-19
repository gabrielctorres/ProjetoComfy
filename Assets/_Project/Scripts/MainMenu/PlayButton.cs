using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour
{
    [SerializeField] private string sceneToLoad;
    public void OnClick()
    {
        if (string.IsNullOrEmpty(sceneToLoad))
        {
            Debug.Log("String da Cena VAZIA!");
            return;
        }
        SceneManager.LoadScene(sceneToLoad);
    }
}
