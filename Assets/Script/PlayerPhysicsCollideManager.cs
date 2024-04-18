

using UnityEngine;

public class PlayerPhysicsCollideManager : SingletonMonobehavior<PlayerPhysicsCollideManager>
{
    public Vector2 CurrentPosition;
    public Vector2 NextPosition;
    public Vector2 CurrentMoveDir;
    public Vector2 CurrentAirDir;

    void FixedUpdate() {
        CurrentPosition = PlayerStatus.CurrentPos;
        NextPosition = PlayerStatus.NextPos;
        CurrentAirDir = PlayerStatus.CurrentAirDirection;
        CurrentMoveDir = PlayerStatus.CurrentMoveDirection;

    }
    protected override void SetInstance() => Instance = this;


}
