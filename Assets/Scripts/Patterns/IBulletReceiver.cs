using Society.Shoot;

using UnityEngine;

namespace Society.Patterns
{
    /// <summary>
    /// наследники этого интерфейса имеют методы, вызываемые при попадании по ним пулей
    /// </summary>
    public interface IBulletReceiver
    {
        void OnBulletEnter(BulletType inputBulletType);
        Transform GetCenter();
    }
}