using Society.Settings;

using UnityEngine;

/// <summary>
/// "����" � ��������� ����������. 
/// ������������� ������ ���������, ��� ������ �� �����������.
/// </summary>
public class BridgeToProjectSettings
{
    public BridgeToProjectSettings() 
        => GameSettings.UpdateSettingsEvent += OnUpdateSettingsEvent;

    private void OnUpdateSettingsEvent()
    {
        //������������� ����� "������ �����"
        Screen.fullScreen = GameSettings.GetIsFullScreen();

        //������������� ����������� ������� ������
        QualitySettings.vSyncCount = GameSettings.GetVSyncIsEnabled() ? 1 : 0;

        //������������� ������� �������
        QualitySettings.SetQualityLevel(
            (int)System.Enum.Parse(typeof(GraphicsLevels), 
            GameSettings.GetQualityLevel().ToString()));

        //������������� ���������� ������
        (int width, int height) = GameSettings.GetAndDescriptScreenResolution();
        Screen.SetResolution(width, height, GameSettings.GetIsFullScreen());
    }

    ~BridgeToProjectSettings()=>    
        GameSettings.UpdateSettingsEvent -= OnUpdateSettingsEvent;
    
}
