using UnityEngine;

public sealed class InventoryDrawer : MonoBehaviour
{
    [SerializeField] private GameObject mainField;

    [Space(15)]
    [SerializeField] private Transform mainContainer;
    [SerializeField] private Transform supportContainer;

    private bool MainFieldEnabled = false;
    public Transform GetMainContainer() => mainContainer;

    public Transform GetSupportContainer() => supportContainer;


    private void Start() => EffectsManager.Instance.Init();


    /// <summary>
    /// смена активности инвентаря
    /// </summary>
    public bool ChangeActiveMainField(bool value)
    {
        MainFieldEnabled = ScreensManager.GetScreen() == null && value;
        mainField.SetActive(MainFieldEnabled);

        EffectsManager.Instance.SetEnableDOF(MainFieldEnabled);
        return MainFieldEnabled;
    }
}
