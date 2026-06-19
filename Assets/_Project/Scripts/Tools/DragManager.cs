using UnityEngine;
using UnityEngine.InputSystem; // IMPORTANTE: Adicionado para usar o New Input System

public class DragManager : MonoBehaviour
{
    public static DragManager Instance { get; private set; }

    [Header("Configurações Visuais")]
    public GameObject dragPreviewPrefab;

    public Ingredient CurrentIngredient { get; private set; }
    private GameObject spawnedPreview;
    private Camera cam;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        if (CurrentIngredient != null && spawnedPreview != null)
        {
            spawnedPreview.transform.position = GetMouseWorldPosition();

            if (Mouse.current != null && Mouse.current.rightButton.wasPressedThisFrame)
            {
                ClearDraggedItem();
            }
        }
    }

    public void SetDraggedItem(Ingredient ingredient, Sprite buttonSprite)
    {
        CurrentIngredient = ingredient;

        if (spawnedPreview == null)
        {
            spawnedPreview = Instantiate(dragPreviewPrefab);
        }

        if (spawnedPreview.TryGetComponent<SpriteRenderer>(out var spriteRenderer))
        {
            spriteRenderer.sprite = CurrentIngredient.dragIcon;
        }
    }

    public void ClearDraggedItem()
    {
        CurrentIngredient = null;
        if (spawnedPreview != null)
        {
            Destroy(spawnedPreview);
            spawnedPreview = null;
        }
    }

    public bool IsDragging() => CurrentIngredient != null;

    private Vector3 GetMouseWorldPosition()
    {
        if (Mouse.current == null) return Vector3.zero;

        // NEW INPUT SYSTEM: Lê a posição do mouse na tela de forma correta
        Vector2 mouseScreenPos = Mouse.current.position.ReadValue();

        Vector3 screenPosWithDepth = new Vector3(mouseScreenPos.x, mouseScreenPos.y, Mathf.Abs(cam.transform.position.z));
        Vector3 worldPos = cam.ScreenToWorldPoint(screenPosWithDepth);
        worldPos.z = 0f;
        return worldPos;
    }
}