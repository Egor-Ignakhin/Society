namespace Debugger
{
    /// <summary>
    /// интерфейс для дебаггерв-частиц
    /// </summary>
    public interface IDebug
    {
        bool Active { get; set; }
        UnityEngine.GameObject gameObject { get; }
        void Activate();
    }
}