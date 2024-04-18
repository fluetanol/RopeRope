using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerStatus
{
    public static float Jump = 30;
    public static float Speed = 15;
    public static Vector2 CurrentPos;
    public static Vector2 NextPos;
    public static Vector2 MoveDirection = Vector2.zero;
    public static Vector2 CurrentMoveDirection = Vector2.zero;
    public static Vector2 CurrentAirDirection = Vector2.zero;
    public static float JumpGravity;
    public static float FallGravity = 4f;
}