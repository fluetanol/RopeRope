using UnityEngine;

public class IdleState : IState
{
    public void PlayState()
    {
        Debug.Log("Idle");
    }
}

public class RunState : IState
{
    public void PlayState()
    {
        Debug.Log("Run");
    }
}

public class JumpState : IState
{

    public void PlayState()
    {
        Debug.Log("Jump");
    }
}


public class DeadState : IState
{
    public void PlayState()
    {
        Debug.Log("Dead");
    }
}