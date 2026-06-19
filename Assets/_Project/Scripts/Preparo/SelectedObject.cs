using UnityEngine;
using DG.Tweening;

public class SelectedObject : MonoBehaviour
{
    [Header("Configurações do Menu")]
    public GameObject toggledMenu;
    public bool ExclusiveWindow = true;
    public bool canToggleMenu = true;

    [Header("Fluxo de Navegação Avançado")]
    [Tooltip("Se configurado, este objeto será DESATIVADO (fechado) ao clicar.")]
    public GameObject objectToClose;

    [Tooltip("Se configurado, este objeto será ATIVADO (aberto) ao clicar.")]
    public GameObject objectToOpen;

    private Vector3 hoverScale = new Vector3(1.05f, 1.05f, 1.05f);
    private float hoverDuration = 0.2f;
    private float clickPunchIntensity = 5f;
    private float clickPunchDuration = 0.3f;

    private Vector3 originalScale;
    private Tween hoverTween;

    private PreparoSystem PreparoManager;
    private PreparoTab myTab;
    private bool isMouseOver = false;

    private Renderer myRenderer;
    private Material localMaterial;

    private int LineXID = Shader.PropertyToID("_LineX");
    private int LineYID = Shader.PropertyToID("_LineY");

    void Start()
    {
        DOTween.Init();
        originalScale = transform.localScale;

        myRenderer = GetComponent<Renderer>();
        if (myRenderer != null)
        {
            localMaterial = myRenderer.material;
        }

        PreparoManager = PreparoSystem.Instance;

        if (toggledMenu && toggledMenu.TryGetComponent<PreparoTab>(out PreparoTab _MyTab))
        {
            myTab = _MyTab;
        }
    }

    private void OnMouseEnter()
    {
        isMouseOver = true;

        if (localMaterial != null)
        {
            localMaterial.SetFloat(LineXID, 0.02f);
            localMaterial.SetFloat(LineYID, 0.02f);
        }

        if (hoverTween != null && hoverTween.IsActive()) hoverTween.Kill();

        hoverTween = transform.DOScale(originalScale * 1.05f, hoverDuration).SetEase(Ease.OutQuad).OnComplete(() => transform.DOScale(originalScale, hoverDuration).SetEase(Ease.InOutQuad));
    }

    private void OnMouseExit()
    {
        isMouseOver = false;

        if (localMaterial != null)
        {
            localMaterial.SetFloat(LineXID, 0f);
            localMaterial.SetFloat(LineYID, 0f);
        }

        if (hoverTween != null && hoverTween.IsActive()) hoverTween.Kill();
        transform.DOScale(originalScale, hoverDuration).SetEase(Ease.InOutQuad);
    }

    private void OnMouseDown()
    {
        if (!canToggleMenu) return;

        transform.DOPunchRotation(new Vector3(0, 0, clickPunchIntensity), clickPunchDuration, 10, 1).SetEase(Ease.OutQuad);


        if (objectToClose != null)
        {
            objectToClose.SetActive(false);
        }

        if (objectToOpen != null)
        {
            objectToOpen.SetActive(true);

            if (ExclusiveWindow && PreparoManager)
            {
                PreparoManager.activeWindow = objectToOpen;
            }
        }

        if (objectToClose == null && objectToOpen == null)
        {
            if (ExclusiveWindow)
            {
                if (PreparoManager && PreparoManager.activeWindow && PreparoManager.activeWindow != toggledMenu)
                {
                    var activeTab = PreparoManager.activeWindow.GetComponent<PreparoTab>();
                    if (activeTab != null && activeTab.busy) return;

                    PreparoManager.activeWindow.SetActive(false);
                }
                if (PreparoManager) PreparoManager.activeWindow = toggledMenu;
            }

            if (myTab && myTab.busy && toggledMenu.activeSelf) { return; }

            if (toggledMenu)
            {
                toggledMenu.SetActive(!toggledMenu.activeInHierarchy);
                if (!toggledMenu.activeSelf && PreparoManager && PreparoManager.activeWindow == toggledMenu)
                {
                    PreparoManager.activeWindow = null;
                }
            }
        }
    }

    public bool IsMouseOver => isMouseOver;

    private void OnDestroy()
    {
        transform.DOKill();
        if (localMaterial != null) Destroy(localMaterial);
    }
}