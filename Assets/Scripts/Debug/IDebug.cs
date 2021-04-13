namespace Debugger
{
    public interface IDebug
    {
        bool Active { get; set; }
        UnityEngine.GameObject gameObject { get;}
    }
}