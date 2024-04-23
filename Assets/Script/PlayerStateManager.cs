using UnityEngine;
using UnityEngine.InputSystem;

public interface IRopeMove{
    public void OnRopeMove(InputAction.CallbackContext callback);
}

//플레이어 state만 조절하는 클래스
public class PlayerStateManager : SingletonMonobehavior<PlayerStateManager>
{
    public EPlayerConditionState PlayerConditionState =EPlayerConditionState.Idle;
    public EPlayerInputState PlayerInputState = EPlayerInputState.Stop;
    private Rigidbody2D _playerRigidBody2d;

    void Start(){
        _playerRigidBody2d = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate(){
        IConditionState cState = PlayerStateFactory.Instance.CreateConditionState((int) PlayerConditionState);
        IInputState iState = PlayerStateFactory.Instance.CreateInputState((int) PlayerInputState);
        iState.PlayInputState(new InputAction.CallbackContext(),transform);
        cState.PlayConditionState(transform, _playerRigidBody2d);
    }

    protected override void SetInstance()=>Instance = this;
}

