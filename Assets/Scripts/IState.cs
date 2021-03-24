
public enum State { unlocked, locked }
public interface IState
{   
    State CurrentState { get; set; }
}
