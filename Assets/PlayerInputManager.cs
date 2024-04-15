using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public interface INormalMove
{
    public void OnMove(InputAction.CallbackContext callback);
    public void OnJump(InputAction.CallbackContext callback);
}

//Player Input을 처리하는 곳
public class PlayerInputManager : SingletonMonobehavior<PlayerInputManager>, INormalMove
{
    public void OnJump(InputAction.CallbackContext callback){
        PlayerStateManager.Instance._playerState = EPlayerState.Jump;
    }

    public void OnMove(InputAction.CallbackContext callback)
    {
        Vector2 moveVector = callback.ReadValue<Vector2>();
        if (moveVector.x == 0) PlayerStateManager.Instance._playerState = EPlayerState.RunStop;
        else {
            PlayerStatus.Instance.direction = moveVector;
            PlayerStateManager.Instance._playerState = EPlayerState.Run;
        }
    }

    protected override void SetInstance() => Instance = this;
}