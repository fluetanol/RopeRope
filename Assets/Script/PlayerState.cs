using System;
using UnityEngine;

public class IdleState : IState
{
    public void PlayState(Transform transform, Rigidbody2D rigidbody2D)
    {
        //Debug.Log(transform.position);
        Debug.Log("idle");
    }
}

public class RunState : IState
{
    public void PlayState(Transform transform, Rigidbody2D rigidbody2D)
    {
        if(PlayerStatus.Instance.Speed < PlayerStatus.Instance.MaxSpeed)  PlayerStatus.Instance.Speed += 0.5f;
        rigidbody2D.MovePosition(
            (Vector2)transform.position + 
            (PlayerStatus.Instance.direction * PlayerStatus.Instance.Speed) *Time.fixedDeltaTime);
        Debug.Log("run");
    }
}


public class RunStopState : IState
{
    public void PlayState(Transform transform, Rigidbody2D rigidbody2D)
    {
        if (PlayerStatus.Instance.Speed > 0) PlayerStatus.Instance.Speed -= 1;

        rigidbody2D.MovePosition(
            (Vector2)transform.position +
            (PlayerStatus.Instance.direction * PlayerStatus.Instance.Speed) * Time.fixedDeltaTime);
        Debug.Log("runstop");
    }
}


public class JumpState : IState
{
    public void PlayState(Transform transform, Rigidbody2D rigidbody2D)
    {
        rigidbody2D.MovePosition(
            (Vector2)transform.position +
            (PlayerStatus.Instance.direction * PlayerStatus.Instance.Speed) * Time.fixedDeltaTime +
            Vector2.up * PlayerStatus.Instance.Jump * Time.fixedDeltaTime);

    }
}


public class DeadState : IState
{
    public void PlayState(Transform transform, Rigidbody2D rigidbody2D)
    {
     
    }
}