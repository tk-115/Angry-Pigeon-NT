public interface IStateSwicher {
    void SwitchState<T>() where T : IState;
}