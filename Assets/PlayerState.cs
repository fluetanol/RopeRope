using System.Collections.Generic;

public interface IState
{
    public IState GetState();
    public void PlayState();
}

public interface IStateFactory{
    public IState CreateState(int eState);
}

public enum EPlayerState {
    Idle,
    Run,
    Jump,
    Dead
}

public class PlayerStateFactory : IStateFactory{
    private Dictionary<EPlayerState, IState> d_playerState;

    public PlayerStateFactory(){
        d_playerState = new();
    }

    public IState CreateState(int nPlayerState){
        EPlayerState ePlayerState = (EPlayerState)nPlayerState;

        if(!d_playerState.ContainsKey(ePlayerState)){
            switch(ePlayerState){
                case EPlayerState.Idle:
                    d_playerState[ePlayerState] = new IdleState();
                    break;
                case EPlayerState.Jump:
                    d_playerState[ePlayerState] = new JumpState();
                    break;
                case EPlayerState.Run:
                    d_playerState[ePlayerState] = new RunState();
                    break;
                case EPlayerState.Dead:
                    d_playerState[ePlayerState] = new DeadState();
                    break;
                default:
                    break;
            }
        }
        return d_playerState[ePlayerState];
    }
}


public class IdleState : IState
{
    public IState GetState()
    {

        throw new System.NotImplementedException();
    }

    public void PlayState()
    {
        throw new System.NotImplementedException();
    }
}

public class RunState : IState
{

    public IState GetState()
    {
        throw new System.NotImplementedException();
    }

    public void PlayState()
    {

    }
}

public class JumpState : IState
{

    public IState GetState()
    {
        throw new System.NotImplementedException();
    }

    public void PlayState()
    {

    }
}


public class DeadState : IState
{
    public IState GetState()
    {
        throw new System.NotImplementedException();
    }

    public void PlayState()
    {

    }
}