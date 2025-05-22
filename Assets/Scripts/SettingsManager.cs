using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class SettingsManager : MonoBehaviour
{
    [Header("Audio")]
    public AudioMixer audioMixer;
    public Slider masterSlider;

    [Header("Display")]
    public TMP_Dropdown aspectRatioDropdown;
    public Button fullscreenButton;
    public Text fullscreenButtonText;

    private readonly Dictionary<string, Vector2Int> aspectRatios = new()
    {
        { "16:9 (1920x1080)", new Vector2Int(1920, 1080) },
        { "4:3 (1024x768)", new Vector2Int(1024, 768) },
        { "21:9 (2560x1080)", new Vector2Int(2560, 1080) },
        { "5:4 (1280x1024)", new Vector2Int(1280, 1024) }
    };

    private void Start()
    {
        // AUDIO
        masterSlider.onValueChanged.AddListener(val => SetVolume("Master", val));

        LoadAudioSettings();

        // DISPLAY
        aspectRatioDropdown.ClearOptions();
        aspectRatioDropdown.AddOptions(new List<string>(aspectRatios.Keys));
        aspectRatioDropdown.onValueChanged.AddListener(OnAspectRatioChanged);
        fullscreenButton.onClick.AddListener(ToggleFullscreen);

        LoadDisplaySettings();
    }

    // Volume handling
    void SetVolume(string param, float value)
    {
        float volume = Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20f;
        audioMixer.SetFloat(param, volume);
        PlayerPrefs.SetFloat(param, value);
    }

    void LoadAudioSettings()
    {
        masterSlider.value = PlayerPrefs.GetFloat("Master", 1f);

        SetVolume("Master", masterSlider.value);
    }

    // Display handling
    void OnAspectRatioChanged(int index)
    {
        string key = aspectRatioDropdown.options[index].text;
        if (aspectRatios.TryGetValue(key, out Vector2Int res))
        {
            Screen.SetResolution(res.x, res.y, Screen.fullScreenMode);
            PlayerPrefs.SetInt("AspectIndex", index);
        }
    }

    void ToggleFullscreen()
    {
        bool isFullscreen = Screen.fullScreenMode == FullScreenMode.Windowed;
        Screen.fullScreenMode = isFullscreen ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
        fullscreenButtonText.text = isFullscreen ? "Exit Fullscreen" : "Go Fullscreen";
        PlayerPrefs.SetInt("IsFullscreen", isFullscreen ? 1 : 0);
    }

    void LoadDisplaySettings()
    {
        int savedIndex = PlayerPrefs.GetInt("AspectIndex", 0);
        bool isFullscreen = PlayerPrefs.GetInt("IsFullscreen", 1) == 1;

        aspectRatioDropdown.value = savedIndex;
        aspectRatioDropdown.RefreshShownValue();

        string key = aspectRatioDropdown.options[savedIndex].text;
        if (aspectRatios.TryGetValue(key, out Vector2Int res))
        {
            Screen.SetResolution(res.x, res.y, isFullscreen);
        }

        Screen.fullScreenMode = isFullscreen ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
        fullscreenButtonText.text = isFullscreen ? "Exit Fullscreen" : "Go Fullscreen";
    }
}
