using System.Collections.Generic;

public interface IStateFactory
{
    public IState CreateState(int eState);
    public IInputState CreateInputState(int eInputState);
    public IConditionState CreateConditionState(int eConditionState);
}

public class PlayerStateFactory : Singleton<PlayerStateFactory>, IStateFactory
{
    private Dictionary<EPlayerState, IState> d_playerState;
    private Dictionary<EPlayerInputState, IInputState> d_playerInputState;
    private Dictionary<EPlayerConditionState, IConditionState> d_playerConditionState;

    public PlayerStateFactory()
    {
        d_playerState = new();
    }

    public IConditionState CreateConditionState(int eConditionState)
    {
        EPlayerConditionState conditionState = (EPlayerConditionState)eConditionState;
        if (!d_playerConditionState.ContainsKey(conditionState))
        {

        }

        return d_playerConditionState[conditionState];

    }

    public IInputState CreateInputState(int eInputState)
    {
        EPlayerInputState  inputState = (EPlayerInputState)eInputState;
        return d_playerInputState[inputState];
    }

    public IState CreateState(int nPlayerState)
    {
        EPlayerState ePlayerState = (EPlayerState)nPlayerState;
        if (!d_playerState.ContainsKey(ePlayerState))
        {
            switch (ePlayerState)
            {
                case EPlayerState.Idle:
                    d_playerState[ePlayerState] = new IdleState();
                    break;
                case EPlayerState.Run:
                    d_playerState[ePlayerState] = new RunState();
                    break;
                case EPlayerState.RunStop:
                    d_playerState[ePlayerState] = new RunStopState();
                    break;
                case EPlayerState.Air:
                    d_playerState[ePlayerState] = new AirState();
                    break;
                case EPlayerState.Dead:
                    d_playerState[ePlayerState] = new DeadState();
                    break;
                case EPlayerState.Jump:
                    d_playerState[ePlayerState] = new AirState();
                    break;
                default:
                    break;
            }
        }
        return d_playerState[ePlayerState];
    }
}