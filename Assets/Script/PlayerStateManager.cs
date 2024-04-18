using UnityEngine;
using UnityEngine.InputSystem;

public interface IRopeMove{
    public void OnRopeMove(InputAction.CallbackContext callback);
}

//플레이어 state만 조절하는 클래스
public class PlayerStateManager : SingletonMonobehavior<PlayerStateManager>
{
    public EPlayerState PlayerState;  
    private Rigidbody2D _playerRigidBody2d;

    void Start(){
        _playerRigidBody2d = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate(){
        IState state = PlayerStateFactory.Instance.CreateState((int)PlayerState);
        state.PlayState(transform, _playerRigidBody2d);
    }

    protected override void SetInstance()=>Instance = this;
}

