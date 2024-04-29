

using UnityEngine;

public class PlayerStatusManager : SingletonMonobehavior<PlayerStatusManager>
{
    public Vector2 CurrentPosition;
    public Vector2 NextPosition;
    public Vector2 CurrentMoveDir;
    public Vector2 CurrentAirDir;
    public Vector2 CurrentInputDir;
    void FixedUpdate() {
        CurrentPosition = PlayerStatus.CurrentPos;
        NextPosition = PlayerStatus.NextPos;
        CurrentAirDir = PlayerStatus.CurrentAirDirection;
        CurrentMoveDir = PlayerStatus.CurrentMoveDirection;
        CurrentInputDir = PlayerStatus.CurrentInputDirection;
        

    }
    protected override void SetInstance() => Instance = this;


}
