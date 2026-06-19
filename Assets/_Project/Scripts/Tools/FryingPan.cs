using UnityEngine;
using DG.Tweening;

public class FryingPan : Tool
{
    [Header("Configurações do Minigame")]
    public float fryingTime = 3f;
    public SpriteRenderer panContentImage;
    public IngredientContainer targetContainer; // Acesso direto ao container igual ao Shaker

    [Header("Animação e Efeitos")]
    public Animator panAnimator;
    public float shakeStrength = 0.05f; // Força leve do shake
    public int shakeVibrato = 10;       // Velocidade de vibração do shake

    private enum PanState { Empty, Frying, Ready }
    private PanState currentState = PanState.Empty;

    private Ingredient ingredientBeingFried;

    // Variáveis para controlar o tempo real decorrido
    private float fryStartTime;
    private float fryProgress = 0f;

    // Variáveis para controlar a animação via código
    private Tween shakeTween;
    private Vector3 originalPosition;

    private void Awake()
    {
        // Salva a posição original para evitar que a panela mude de lugar após o shake
        originalPosition = transform.localPosition;
    }

    private void Update()
    {
        // Se estiver fritando, atualiza o progresso com base no tempo do jogo
        if (currentState == PanState.Frying)
        {
            fryProgress = Time.time - fryStartTime;

            if (fryProgress >= fryingTime)
            {
                FinishFrying();
            }
        }
    }

    private void OnEnable()
    {
        // Se ele foi desligado enquanto fritava, recalculamos o tempo para ver se já terminou
        if (currentState == PanState.Frying)
        {
            fryProgress = Time.time - fryStartTime;

            if (fryProgress >= fryingTime)
            {
                FinishFrying();
            }
            else
            {
                // Se ainda não terminou, mantém o visual ativo do item
                UpdateVisuals(isFrying: true);

                // Retoma as animações pelo tempo restante
                StartFryingAnimations(fryingTime - fryProgress);
            }
        }
    }

    private void OnDisable()
    {
        // Garante que o Tween seja cancelado caso o objeto seja desativado enquanto frita
        StopFryingAnimations();
    }

    // --- INTERAÇÃO COM O MOUSE (IGUAL AO SHAKER) ---
    protected override void OnMouseEnter()
    {
        // Só aceita se a panela não estiver ocupada fritando e se houver um item sendo arrastado
        if (currentState == PanState.Frying || !DragManager.Instance.IsDragging()) return;

        // Verifica se o container de destino tem espaço antes de aceitar
        if (targetContainer != null && targetContainer.ingredients.Count >= targetContainer.maxIngredients) return;

        Ingredient currentIngredient = DragManager.Instance.CurrentIngredient;

        // Se quiser adicionar alguma validação extra de tipo no futuro (como o IsValidForShaker), pode colocar aqui.
        // Por enquanto, aceita o ingrediente direto e executa:
        Execute(currentIngredient);
    }

    protected override void Execute(Ingredient ingredient)
    {
        ingredientBeingFried = ingredient;

        DragManager.Instance.ClearDraggedItem();

        fryStartTime = Time.time;
        fryProgress = 0f;
        currentState = PanState.Frying;

        UpdateVisuals(isFrying: true);

        StartFryingAnimations(fryingTime);
    }

    private void FinishFrying()
    {
        currentState = PanState.Ready;

        UpdateVisuals(isFrying: false);

        StopFryingAnimations();

        if (targetContainer != null && ingredientBeingFried != null)
        {
            targetContainer.AddIngredient(ingredientBeingFried);
        }

        ResetPan();
    }

    private void UpdateVisuals(bool isFrying)
    {
        if (panContentImage == null) return;

        if (isFrying && ingredientBeingFried != null)
        {
            panContentImage.gameObject.SetActive(true);
            panContentImage.sprite = ingredientBeingFried.icon;
            panContentImage.color = new Color(0.6f, 0.6f, 0.6f);
        }
        else
        {
            panContentImage.gameObject.SetActive(false);
        }
    }

    private void ResetPan()
    {
        ingredientBeingFried = null;
        currentState = PanState.Empty;

        if (panContentImage != null)
        {
            panContentImage.gameObject.SetActive(false);
        }
    }

    private void StartFryingAnimations(float duration)
    {
        if (panAnimator != null)
        {
            panAnimator.SetBool("IsFrying", true);
        }

        shakeTween = transform.DOShakePosition(duration, strength: shakeStrength, vibrato: shakeVibrato, randomness: 90, snapping: false, fadeOut: false);
    }

    private void StopFryingAnimations()
    {
        if (panAnimator != null)
        {
            panAnimator.SetBool("IsFrying", false);
        }

        if (shakeTween != null && shakeTween.IsActive())
        {
            shakeTween.Kill();
        }

        transform.localPosition = originalPosition;
    }
}