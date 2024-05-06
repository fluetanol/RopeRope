using System.Collections.Generic;

public interface IStateFactory
{
    public IInputState CreateInputState(int eInputState);
    public IConditionState CreateConditionState(int eConditionState);
}

public class PlayerStateFactory : Singleton<PlayerStateFactory>, IStateFactory
{
    private Dictionary<EPlayerInputState, IInputState> d_playerInputState;
    private Dictionary<EPlayerConditionState, IConditionState> d_playerConditionState;

    public PlayerStateFactory()
    {
        d_playerInputState =  new();
        d_playerConditionState = new();
    }

    public IConditionState CreateConditionState(int eConditionState)
    {
        EPlayerConditionState conditionState = (EPlayerConditionState)eConditionState;
        if (!d_playerConditionState.ContainsKey(conditionState))
        {
            switch (conditionState)
            {
                case EPlayerConditionState.Idle:
                    d_playerConditionState[conditionState] = new IdleState();
                    break;
                case EPlayerConditionState.Air:
                    d_playerConditionState[conditionState] = new AirState();
                    break;
                case EPlayerConditionState.Koyote:
                    d_playerConditionState[conditionState] = new KoyoteState();
                    break;
                case EPlayerConditionState.Rope:
                    d_playerConditionState[conditionState] =  new RopeState();
                    break;
                case EPlayerConditionState.Dead:
                    d_playerConditionState[conditionState] =  new DeadState();
                    break;
                default:
                    break;
            }
        }

        return d_playerConditionState[conditionState];

    }

    public IInputState CreateInputState(int eInputState)
    {
        EPlayerInputState inputState = (EPlayerInputState)eInputState;
        if (!d_playerInputState.ContainsKey(inputState))
        {
            switch (inputState)
            {
                case EPlayerInputState.Stop:
                    d_playerInputState[inputState] = new StopState();
                    break;
                case EPlayerInputState.Run:
                    d_playerInputState[inputState] = new RunState();
                    break;
                case EPlayerInputState.Jump:
                    d_playerInputState[inputState] = new JumpState();
                    break;
                case EPlayerInputState.RopeMove:
                    d_playerInputState[inputState] = new RopeMoveState();
                    break;
                default:
                    break;
            }
        }
        return d_playerInputState[inputState];
    }

}