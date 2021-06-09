using MenuScripts.PauseMenu;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMain : MonoBehaviour
{    
    [SerializeField] private Slider fovSlider;
    [SerializeField] private TextMeshProUGUI fovText;
    private static CurrentGameSettings currentGameSettings;
    public static CurrentGameSettings GetCurrentGameSettings() => currentGameSettings;
    private readonly string pathForSettings = Directory.GetCurrentDirectory() + "\\Saves\\Settings.json";
    [SerializeField] private Toggle bloomToggle;

    [SerializeField] private TextMeshProUGUI sensivityText;
    [SerializeField] private Slider sensivitySlider;
    [SerializeField] private Toggle reloadCABToggle;

    private void Start()
    {
        LoadData();

        fovSlider.value = (currentGameSettings.FOV - currentGameSettings.minFov) / (currentGameSettings.maxFov - currentGameSettings.minFov);
        fovText.SetText(currentGameSettings.FOV.ToString());

        sensivitySlider.value = (float)currentGameSettings.Sensivity / 10;
        sensivityText.SetText(currentGameSettings.Sensivity.ToString());

        bloomToggle.isOn = currentGameSettings.BloomEnabled;
        reloadCABToggle.isOn = currentGameSettings.reloadEffectEnabled;

        sensivitySlider.onValueChanged.AddListener(ChangeSensivitySlider);
        fovSlider.onValueChanged.AddListener(ChangeFovSlider);
        bloomToggle.onValueChanged.AddListener(SetActiveGlobalBloom);
        reloadCABToggle.onValueChanged.AddListener(SetActiveReloadCAB);
    }


    public void SetActiveGlobalBloom(bool isOn)
    {
        currentGameSettings.BloomEnabled = isOn;
    }
    public void SetActiveReloadCAB(bool isOn)
    {
        currentGameSettings.reloadEffectEnabled = isOn;
    }
    public void ChangeFovSlider(float v)
    {
        currentGameSettings.FOV = currentGameSettings.minFov + ((currentGameSettings.maxFov - currentGameSettings.minFov) * v);
        currentGameSettings.FOV = (float)System.Math.Round(currentGameSettings.FOV, 1);// округление до нормальных значений
        fovText.SetText(currentGameSettings.FOV.ToString());        
    }
    public void ChangeSensivitySlider(float v)
    {
        currentGameSettings.Sensivity = (int)(v * 10);
        sensivityText.SetText(currentGameSettings.Sensivity.ToString());        
    }
    private void LoadData()
    {
        try
        {
            string data = File.ReadAllText(pathForSettings);
            currentGameSettings = JsonUtility.FromJson<CurrentGameSettings>(data);
        }
        catch
        {
            currentGameSettings = new CurrentGameSettings();
        }
        if (currentGameSettings == null)
            currentGameSettings = new CurrentGameSettings();
    }
    private void OnDestroy()
    {
        SaveData();
        sensivitySlider.onValueChanged.RemoveListener(ChangeSensivitySlider);
        fovSlider.onValueChanged.RemoveListener(ChangeFovSlider);
    }

    private void SaveData()
    {
        string data = JsonUtility.ToJson(currentGameSettings, true);
        File.WriteAllText(pathForSettings, data);
    }

}
