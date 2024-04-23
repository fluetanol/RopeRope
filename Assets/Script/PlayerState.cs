using UnityEngine;
using UnityEngine.InputSystem;

public class IdleState : IConditionState
{
    public void PlayConditionState(Transform transform, Rigidbody2D rigidbody2D){

    }

}


public class AirState : IConditionState
{
    public void PlayConditionState(Transform transform, Rigidbody2D rigidbody2D)
    {

    }
}

public class DeadState : IConditionState
{
    public void PlayConditionState(Transform transform, Rigidbody2D rigidbody2D)
    {

    }
}


public class StopState : IInputState
{
    public void PlayInputState(InputAction.CallbackContext context, Transform transform)
    {
    }
}

public class RunState : IInputState
{
    public void PlayInputState(InputAction.CallbackContext context, Transform transform){
    }
}



public class JumpState : IInputState
{
    public void PlayInputState(InputAction.CallbackContext context, Transform transform)
    {
    }
}

