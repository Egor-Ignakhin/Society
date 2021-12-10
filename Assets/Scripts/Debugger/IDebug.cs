namespace Society.Debugger
{
    /// <summary>
    /// интерфейс для дебаггеров-компонент
    /// </summary>
    public interface IDebug
    {
        bool IsActive { get; set; }
        UnityEngine.GameObject gameObject { get; }
        void Activate();
    }
}