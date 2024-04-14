using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;



public interface INormalMove{
    public void OnMove(InputAction.CallbackContext callback);
    public void OnJump(InputAction.CallbackContext callback);
}

public interface IRopeMove{
    public void OnRopeMove(InputAction.CallbackContext callback);
}

public class PlayerManager : SingletonMonobehavior<PlayerManager>, INormalMove
{
    private EPlayerState _playerState = EPlayerState.Idle;

    // Update is called once per frame
    void Update(){
        PlayerStateFactory.Instance.CreateState((int)_playerState).PlayState();
    }

    public void OnJump(InputAction.CallbackContext callback){
        _playerState = EPlayerState.Jump;
    }

    public void OnMove(InputAction.CallbackContext callback){
        Vector2 moveVector = callback.ReadValue<Vector2>();
        if(moveVector.x == 0) _playerState = EPlayerState.Idle;
        else _playerState = EPlayerState.Run;
    }

    protected override void SetInstance(){
        Instance = this;
    }
}
