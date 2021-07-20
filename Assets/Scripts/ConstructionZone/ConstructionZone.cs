using System.Collections.Generic;
using UnityEngine;
namespace CampSystem
{
    sealed class ConstructionZone : MonoBehaviour, IGameScreen
    {
        [SerializeField] private List<SectionGameobjects> levelsOfProtection;
        [SerializeField] private int currentSelection;
        private int previousSelection = -1; // Избавляюсь от нулевого значения        
        private RadialMenu radialMenu;
        private Collider playerCollider;
        private bool playerIsIntersected = false;

        [System.Serializable]
        public class SectionGameobjects
        {
            [SerializeField] private string name; // дополнительное поле, чтобы в инсекторе отобразить имя массива для удобства
            [SerializeField] public List<GameObject> gameObjects; // Объекты на сцене, которые будут включаться при выборе данной секции
        }
        private void Start()
        {       
            radialMenu = RadialMenu.Instance; // Получаем компонент внутри канваса
            radialMenu.gameObject.SetActive(false);
            foreach (SectionGameobjects section in levelsOfProtection) // Для каждого уровня защиты
            {
                foreach (GameObject obj in section.gameObjects) // Отключаем каждый объект внутри этого уровня
                {
                    obj.SetActive(false);
                }
            }
            playerCollider = FindObjectOfType<FirstPersonController>().GetCollider();
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other == playerCollider)
                playerIsIntersected = true;
        }
        private void OnTriggerExit(Collider other)
        {
            if (other == playerCollider)
                playerIsIntersected = false;
        }
        private void Update()
        {
            CheckInput();
            EnablePrefabs();
        }
        private void CheckInput()
        {

            if (Input.GetKeyDown(KeyCode.F))
            {
                if (!playerIsIntersected)
                    return;
                if (!ScreensManager.HasActiveScreen())
                    Show();
            }
        }
        private void EnablePrefabs()
        {            
            if (!radialMenu.gameObject.activeInHierarchy)
                return;
            if (!Input.GetMouseButtonDown(0))
                return;

            currentSelection = radialMenu.GetCurrentSelection(); // При нажатии получаем выбранную секцию
            if (!((previousSelection + 1 == currentSelection) && (currentSelection > previousSelection))) // Если номер выбранной секции больше предыдущей и номер предыдущей + 1 = выбранной секции
                return;
            foreach (GameObject protectionObject in levelsOfProtection[currentSelection].gameObjects)
            {
                protectionObject.SetActive(true);
            }
            previousSelection = currentSelection; // И присваеваем эту секцию к прошлой секции
        }
        public void Show()
        {
            ScreensManager.SetScreen(this);
            radialMenu.gameObject.SetActive(true);
        }
        public void Hide()
        {
            ScreensManager.SetScreen(null);
            radialMenu.gameObject.SetActive(false);
        }

        public KeyCode HideKey() => KeyCode.Escape;
    }
}