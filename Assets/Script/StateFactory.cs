using System.Collections.Generic;

public interface IStateFactory
{
    public IState CreateState(int eState);
}

public class PlayerStateFactory : Singleton<PlayerStateFactory>, IStateFactory
{
    private Dictionary<EPlayerState, IState> d_playerState;

    public PlayerStateFactory()
    {
        d_playerState = new();
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