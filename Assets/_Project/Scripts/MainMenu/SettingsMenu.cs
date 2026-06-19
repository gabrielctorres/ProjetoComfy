using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private AudioMixer mixer;
    public GameObject mainWindow;

    void Start()
    {
        RefreshSettings();
        mainWindow.SetActive(false);
    }

    public void Toggle()
    {
        mainWindow.SetActive(!mainWindow.activeSelf);
    }


    public void RefreshSettings()
    {
        volumeSlider.value = Settings.Volume;
    }

    public void Apply()
    {
        Settings.Volume = volumeSlider.value;

        mixer.SetFloat("Master", Mathf.Log10(Settings.Volume));
    }

}
