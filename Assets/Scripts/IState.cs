
public enum State { locked, unlocked }
public interface IState
{   
    State CurrentState { get; set; }
}
