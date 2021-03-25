
public enum State { unlocked, locked, nullable }
public interface IState
{   
    State CurrentState { get; set; }
}
