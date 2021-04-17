using UnityEngine;

namespace Debugger
{
    sealed class ConsoleCreator : MonoBehaviour// объект с этим классом создаёт экземпляр дебаггера
    {
        private void Awake()
        {
            Init();
        }
        private void Init()
        {
            Instantiate(Resources.Load<GameObject>("Debug\\[Canvas] Debug"), transform);
        }
    }
}