using System;
using UnityEditor;
using UnityEngine;

public class ToggleVisibilityButton : MonoBehaviour
{
    public GameObject toggledMenu;
    public bool ExclusiveWindow = true;
    PreparoSystem PreparoManager;

    PreparoTab myTab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        PreparoManager = GetComponentInParent<PreparoSystem>();
        if (toggledMenu.TryGetComponent<PreparoTab>(out PreparoTab _MyTab))
            { 

            myTab = _MyTab;

            }
    }

    public void OnClick()
    {
        if (ExclusiveWindow) { 
            if (PreparoManager.activeWindow && PreparoManager.activeWindow != toggledMenu)
            {
                if (PreparoManager.activeWindow.GetComponent<PreparoTab>().busy)
                {
                    return;
                }
                PreparoManager.activeWindow.SetActive(false);
            }
            PreparoManager.activeWindow = toggledMenu;
        }

        if (myTab && myTab.busy && toggledMenu.activeSelf) { return; }
        toggledMenu.SetActive(!toggledMenu.activeInHierarchy);
        if (!toggledMenu.activeSelf && PreparoManager.activeWindow == toggledMenu) { PreparoManager.activeWindow = null; }
    }

}
