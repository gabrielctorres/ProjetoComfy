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
        UpdateMixerVolume(Settings.Volume);
    }

    public void Apply()
    {
        Settings.Volume = volumeSlider.value;
        UpdateMixerVolume(Settings.Volume);
    }
    public void OnVolumeSliderChanged(float value)
    {
        UpdateMixerVolume(value);
    }
    private void UpdateMixerVolume(float value)
    {
        if (mixer == null) return;

        if (value <= 0.0001f)
        {
            mixer.SetFloat("Master", -80f);
        }
        else
        {
            float dbVolume = Mathf.Log10(value) * 20f;

            dbVolume = Mathf.Clamp(dbVolume, -80f, 20f);

            mixer.SetFloat("Master", dbVolume);
        }
    }
}