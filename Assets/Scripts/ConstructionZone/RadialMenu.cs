using UnityEngine;

namespace CampSystem
{
    sealed class RadialMenu : MonoBehaviour
    {
        [SerializeField] private int numberOfSelections = 12;
        [SerializeField] private RadialSelection[] radialSelections;
        private Vector2 normilizedMousePosition;
        private float currentAngle;
        private int currentSelection;
        private int previousSelection;
        private RadialSelection currentRadialSelection;
        private RadialSelection previousRadialSelection;

        private void Start()
        {
            gameObject.SetActive(false);
        }
        private void Update()
        {
            wtf();
        }
        private void wtf()
        {
            normilizedMousePosition = new Vector2(Screen.width / 2 - Input.mousePosition.x, Screen.height / 2 - Input.mousePosition.y);
            currentAngle = Mathf.Atan2(normilizedMousePosition.x, normilizedMousePosition.y) * Mathf.Rad2Deg;
            currentAngle = (currentAngle + 360) % 360;
            currentSelection = (int)(currentAngle / (360 / numberOfSelections));

            if (currentSelection != previousSelection)
            {
                previousRadialSelection = radialSelections[previousSelection];
                previousRadialSelection.Deselected();
                previousSelection = currentSelection;

                currentRadialSelection = radialSelections[currentSelection];
                currentRadialSelection.Selected();
            }
        }
        public int GetCurrentSelection() => currentSelection;        
    }
}