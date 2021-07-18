using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialMenu : MonoBehaviour, IGameScreen
{
    [SerializeField] private int numberOfSelections = 12;
    [SerializeField] private RadialSelection[] radialSelections;
    private Vector2 normilizedMousePosition;
    private float currentAngle;
    private int currentSelection;
    private int previousSelection;
    private RadialSelection currentRadialSelection;
    private RadialSelection previousRadialSelection;
    private Canvas RadialMenuObj;

    private void Start()
    {
        RadialMenuObj = gameObject.GetComponent<Canvas>(); 
    }

    private void Update()
    {
        normilizedMousePosition = new Vector2(Screen.width / 2 - Input.mousePosition.x, Screen.height / 2 - Input.mousePosition.y);
        currentAngle = Mathf.Atan2(normilizedMousePosition.x, normilizedMousePosition.y) * Mathf.Rad2Deg;
        currentAngle = (currentAngle + 360) % 360;
        currentSelection = (int) (currentAngle / (360 / numberOfSelections));

        if(currentSelection != previousSelection)
        {
            previousRadialSelection = radialSelections[previousSelection];
            previousRadialSelection.Deselected();
            previousSelection = currentSelection;

            currentRadialSelection = radialSelections[currentSelection];
            currentRadialSelection.Selected();
        }
    }
    public int GetCurrentSelection()
    {
        return currentSelection;
    }

    public void Hide()
    {
        if (ScreensManager.HasActiveScreen() == false) 
        {
            ScreensManager.SetScreen(this);
            RadialMenuObj.enabled = !RadialMenuObj.enabled;
        }
    }

    public KeyCode HideKey() => KeyCode.F;

}
