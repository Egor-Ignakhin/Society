using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConstructionZone : MonoBehaviour
{
    [SerializeField] private List<SectionGameobjects> levelsOfProtection;
    [SerializeField] private int currentSelection;
    private int previousSelection = -1; // Избавляюсь от нулевого значения
    private Canvas radialMenuCanvas;
    private RadialMenu radialMenu;

    [System.Serializable]
    public class SectionGameobjects
    {
        [SerializeField] private string name; // дополнительное поле, чтобы в инсекторе отобразить имя массива для удобства
        [SerializeField] public List<GameObject> gameObjects; // Объекты на сцене, которые будут включаться при выборе данной секции
    }
    private void Start()
    {
        radialMenuCanvas = GameObject.Find("[Canvas] Radial menu(Clone)").GetComponent<Canvas>(); // Находим Канвас отвечающий за Радиальное меню
        radialMenuCanvas.enabled = false;
        radialMenu = GameObject.FindObjectOfType<RadialMenu>(); // Получаем компонент внутри канваса
        foreach (SectionGameobjects section in levelsOfProtection) // Для каждого уровня защиты
        {
            foreach (GameObject obj in section.gameObjects) // Отключаем каждый объект внутри этого уровня
            {
                obj.SetActive(false);
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            //if(radialMenu.HideKey())
            //{
            //    radialMenu.Hide();
            //    //radialMenuCanvas.enabled = !radialMenuCanvas.enabled;
            //}
            if (Input.GetKeyDown(KeyCode.F))
            {
                radialMenu.Hide();
                //radialMenuCanvas.enabled = !radialMenuCanvas.enabled;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            radialMenuCanvas.enabled = false;
        }
    }
    private void Update()
    {
        if(radialMenuCanvas.enabled == true)
        {
            if(Input.GetMouseButtonDown(0))
            {
                currentSelection = radialMenu.GetCurrentSelection(); // При нажатии получаем выбранную секцию
                if ((previousSelection + 1 == currentSelection) && (currentSelection > previousSelection)) // Если номер выбранной секции больше предыдущей и номер предыдущей + 1 = выбранной секции
                {
                    foreach (GameObject protectionObject in levelsOfProtection[currentSelection].gameObjects)
                    {
                        protectionObject.SetActive(true);
                    }
                    previousSelection = currentSelection; // И присваеваем эту секцию к прошлой секции
                }
            }
        }
    }
}
