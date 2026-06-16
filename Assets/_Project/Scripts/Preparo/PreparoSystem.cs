using UnityEngine;

public class PreparoSystem : MonoBehaviour
{

    public static PreparoSystem Instance { get; private set; }
    public GameObject activeWindow;


    private void Awake()
    {
        // só pod ter um
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }


}
