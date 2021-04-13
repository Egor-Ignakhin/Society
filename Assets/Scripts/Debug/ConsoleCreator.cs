using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Debugger
{
    sealed class ConsoleCreator : MonoBehaviour
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