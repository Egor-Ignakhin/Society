public static class EventHandlers
{
    public delegate void EventHandler();
}
public interface IDelayable
{    
    event EventHandlers.EventHandler FinishPart;
}
