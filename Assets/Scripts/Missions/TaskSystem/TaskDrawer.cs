using UnityEngine;
namespace Society.Missions.TaskSystem
{
    /// <summary>
    /// Класс рисует текст текущей входящей задачи
    /// </summary>
    internal sealed class TaskDrawer : MonoBehaviour
    {
        [SerializeField] private TMPro.TextMeshProUGUI text;
        [SerializeField] private GameObject taskField;
        public void DrawNewTask(string c)
        {
            text.SetText(c);
        }

        internal void SetVisible(bool v) => taskField.SetActive(v);
    }
}