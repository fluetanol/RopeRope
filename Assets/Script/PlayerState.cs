using UnityEngine;

public class IdleState : IState
{
    public void PlayState(Transform transform, Rigidbody2D rigidbody2D)
    {
    }


}

public class RunState : IState
{
    public void PlayState(Transform transform, Rigidbody2D rigidbody2D)
    {
    }
}


public class RunStopState : IState
{
    public void PlayState(Transform transform, Rigidbody2D rigidbody2D)
    {

    }

}

public class AirState : IState
{   
    public void PlayState(Transform transform, Rigidbody2D rigidbody2D)
    {
    }
}



public class JumpState : IState
{
    public void PlayState(Transform transform, Rigidbody2D rigidbody2D)
    {
    }
}


public class DeadState : IState
{
    public void PlayState(Transform transform, Rigidbody2D rigidbody2D)
    {
     
    }
}