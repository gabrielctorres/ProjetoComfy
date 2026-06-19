using UnityEngine;
using DG.Tweening;

public class SideMenuAnimation : MonoBehaviour
{
    public enum MenuControlMode { ByButtonToggle, ByEnableDisable }

    [Header("Modo de Funcionamento")]
    [SerializeField] private MenuControlMode controlMode = MenuControlMode.ByButtonToggle;

    [Header("Configurações de Posição")]
    [SerializeField] private float offScreenX = 800f;
    [SerializeField] private float onScreenX = 400f;

    [Header("Configurações da Animação")]
    [SerializeField] private float duration = 0.35f;
    [SerializeField] private Ease openEase = Ease.OutCubic;
    [SerializeField] private Ease closeEase = Ease.InCubic;

    private RectTransform rectTransform;
    private Tween activeTween;
    private bool isOpen = false;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        if (rectTransform == null) rectTransform = GetComponent<RectTransform>();

        if (controlMode == MenuControlMode.ByEnableDisable)
        {
            AnimateOpen();
        }
        else
        {
            rectTransform.anchoredPosition = new Vector2(isOpen ? onScreenX : offScreenX, rectTransform.anchoredPosition.y);
        }
    }

    public void ToggleMenuViaButton()
    {
        if (controlMode != MenuControlMode.ByButtonToggle) return;

        isOpen = !isOpen;

        if (isOpen)
        {
            AnimateOpen();
        }
        else
        {
            AnimateClose(disableOnComplete: false);
        }
    }


    public void HideMenu()
    {
        if (controlMode != MenuControlMode.ByEnableDisable) return;

        AnimateClose(disableOnComplete: true);
    }

    private void AnimateOpen()
    {
        KillActiveTween();

        if (rectTransform.anchoredPosition.x == onScreenX)
        {
            rectTransform.anchoredPosition = new Vector2(offScreenX, rectTransform.anchoredPosition.y);
        }

        activeTween = rectTransform.DOAnchorPosX(onScreenX, duration).SetEase(openEase).SetUpdate(true);
    }

    private void AnimateClose(bool disableOnComplete)
    {
        KillActiveTween();

        activeTween = rectTransform.DOAnchorPosX(offScreenX, duration).SetEase(closeEase).SetUpdate(true).OnComplete(() =>
            {
                if (disableOnComplete)
                {
                    gameObject.SetActive(false);
                }
            });
    }

    private void OnDisable()
    {
        if (controlMode == MenuControlMode.ByEnableDisable)
        {
            HideMenu();
        }
        KillActiveTween();
    }

    private void KillActiveTween()
    {
        if (activeTween != null && activeTween.IsActive())
        {
            activeTween.Kill();
        }
    }
}