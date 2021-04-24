using UnityEngine;

public sealed class TaskDrawer : MonoBehaviour// класс отрисовывает задачи
{
    [SerializeField] private TMPro.TextMeshProUGUI text;

    public void DrawNewTask(string c)
    {
        text.SetText(c);
        gameObject.SetActive(c != string.Empty);

    }
}
