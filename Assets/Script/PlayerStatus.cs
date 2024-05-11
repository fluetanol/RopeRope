using UnityEngine;

public static class PlayerStatus
{
    public static float Jump = 26;
    public static float Speed = 23;
    public static Vector2 CurrentPos;
    public static Vector2 NextPos;
    public static Vector2 MoveDirection = Vector2.zero;
    public static Vector2 CurrentMoveDirection = Vector2.zero;
    public static Vector2 CurrentJumpDirection = Vector2.zero;
    public static Vector2 CurrentAirDirection = Vector2.zero;
    public static Vector2 CurrentInputDirection = Vector2.zero;
    public static float JumpGravity;
    public static float FallGravity = 4.25f;
    public static float KoyoteTime = 0.2f;
    public static float RopeCoolTime = 0.5f;
}