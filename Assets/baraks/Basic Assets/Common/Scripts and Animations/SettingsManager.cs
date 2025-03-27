using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class SettingsManager : MonoBehaviour
{
    [Header("Audio Settings")]
    public Slider volumeSlider;
    public AudioMixer audioMixer;

    [Header("Screen Settings")]
    public  TMP_Dropdown screenModeDropdown;

    [Header("Keybinding Settings")]
    public Button forwardKeyButton;
    public Button backwardKeyButton;
    public Button leftKeyButton;
    public Button rightKeyButton;
    public Button interactKeyButton;
    public Button doorKeyButton; 

    private Dictionary<string, KeyCode> keyBindings = new Dictionary<string, KeyCode>()
    {
        { "Forward", KeyCode.W },
        { "Backward", KeyCode.S },
        { "Left", KeyCode.A },
        { "Right", KeyCode.D },
        { "Interact", KeyCode.E },
        { "Door", KeyCode.Mouse0 } 
    };

    private string keyToRebind = "";

    void Start()
    {
        LoadSettings();

        // Add listeners for UI elements
        volumeSlider.onValueChanged.AddListener(SetVolume);
        screenModeDropdown.onValueChanged.AddListener(SetScreenMode);

        forwardKeyButton.onClick.AddListener(() => StartRebind("Forward"));
        backwardKeyButton.onClick.AddListener(() => StartRebind("Backward"));
        leftKeyButton.onClick.AddListener(() => StartRebind("Left"));
        rightKeyButton.onClick.AddListener(() => StartRebind("Right"));
        interactKeyButton.onClick.AddListener(() => StartRebind("Interact"));
        doorKeyButton.onClick.AddListener(() => StartRebind("Door"));

        UpdateKeyTexts();
    }

    void LoadSettings()
    {
        // Load volume
        float savedVolume = PlayerPrefs.GetFloat("Volume", 0.5f);
        volumeSlider.value = savedVolume;
        SetVolume(savedVolume);

        // Load screen mode
        int savedScreenMode = PlayerPrefs.GetInt("ScreenMode", 0);
        screenModeDropdown.value = savedScreenMode;
        SetScreenMode(savedScreenMode);

        // Load key bindings
        foreach (string key in keyBindings.Keys)
        {
            if (PlayerPrefs.HasKey(key))
            {
                keyBindings[key] = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString(key));
            }
        }
    }

    // AUDIO SETTINGS
    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("Volume", volume);
    }

    // SCREEN SETTINGS
    public void SetScreenMode(int index)
    {
        switch (index)
        {
            case 0: Screen.fullScreenMode = FullScreenMode.FullScreenWindow; break;
            case 1: Screen.fullScreenMode = FullScreenMode.MaximizedWindow; break;
            case 2: Screen.fullScreenMode = FullScreenMode.Windowed; break;
        }
        PlayerPrefs.SetInt("ScreenMode", index);
    }

    // KEYBINDING SYSTEM
    public void StartRebind(string key)
    {
        keyToRebind = key;
        StartCoroutine(WaitForKeyPress());
    }

    private IEnumerator WaitForKeyPress()
    {
        yield return null; // Wait for next frame to prevent button click interference

        while (!Input.anyKeyDown)
        {
            yield return null;
        }

        foreach (KeyCode key in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(key))
            {
                keyBindings[keyToRebind] = key;
                PlayerPrefs.SetString(keyToRebind, key.ToString());
                UpdateKeyTexts();
                break;
            }
        }

        keyToRebind = "";
    }

    void UpdateKeyTexts()
    {
        forwardKeyButton.GetComponentInChildren<Text>().text = "Forward: " + keyBindings["Forward"];
        backwardKeyButton.GetComponentInChildren<Text>().text = "Backward: " + keyBindings["Backward"];
        leftKeyButton.GetComponentInChildren<Text>().text = "Left: " + keyBindings["Left"];
        rightKeyButton.GetComponentInChildren<Text>().text = "Right: " + keyBindings["Right"];
        interactKeyButton.GetComponentInChildren<Text>().text = "Interact: " + keyBindings["Interact"];
        doorKeyButton.GetComponentInChildren<Text>().text = "Door: " + keyBindings["Door"];
    }

    public KeyCode GetKey(string action)
    {
        return keyBindings.ContainsKey(action) ? keyBindings[action] : KeyCode.None;
    }

    // BACK BUTTON
    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
