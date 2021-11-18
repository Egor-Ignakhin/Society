using System.Collections.Generic;

namespace Society.Localization
{
    [System.Serializable]
    public sealed class Mission
    {
        public string Title;
        public List<string> Tasks;// пути к задачам
        public string GetTask(int taskId) => Tasks[taskId];
        public int GetNumberTasks() => Tasks.Count;
    }
}