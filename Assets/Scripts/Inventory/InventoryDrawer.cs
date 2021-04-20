﻿using UnityEngine;

public sealed class InventoryDrawer : Singleton<InventoryDrawer>
{
    [SerializeField] private GameObject mainField;

    [Space(15)]
    [SerializeField] private Transform mainContainer;
    [SerializeField] private Transform supportContainer;

    public static bool MainFieldEnabled { get; private set; } = false;
    private delegate void EventHandler();
    private static event EventHandler mainFieldActiveEvent;
    public Transform GetMainContainer()
    {
        return mainContainer;
    }
    public Transform GetSupportContainer()
    {
        return supportContainer;
    }
    private void OnEnable()
    {
        mainFieldActiveEvent += SetActiveMainField;
    }
    private void Start()
    {
        mainFieldActiveEvent?.Invoke();
    }
    /// <summary>
    /// включение видимости инвентаря
    /// </summary>
    private void SetActiveMainField()
    {
        mainField.SetActive(MainFieldEnabled);
    }
    /// <summary>
    /// смена активности инвентаря
    /// </summary>
    public static void ChangeActiveMainField()
    {
        MainFieldEnabled = !Shoots.GunAnimator.Instance.isAiming && !MainFieldEnabled;
        mainFieldActiveEvent?.Invoke();
    }
    private void OnDisable()
    {
        mainFieldActiveEvent -= SetActiveMainField;
    }
}
