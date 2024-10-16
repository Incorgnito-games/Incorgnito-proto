namespace Incorgnito.scripts.components.state;

public interface IState
{
    public void Enter();

    public void Exit();

    public void UpdateProcess(double delta);

    public void UpdatePhysicsProcess(double delta);

}