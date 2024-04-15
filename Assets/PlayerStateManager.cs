using UnityEngine;
using UnityEngine.InputSystem;

public interface IRopeMove{
    public void OnRopeMove(InputAction.CallbackContext callback);
}

//플레이어 state만 조절하는 클래스
public class PlayerStateManager : SingletonMonobehavior<PlayerStateManager>
{
    public EPlayerState _playerState = EPlayerState.Idle;   
    private Rigidbody2D _playerRigidBody2d;

    new void Awake(){
        base.Awake();
        SetInstance();
    }

    void Start(){
        _playerRigidBody2d = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate(){
        IState state = PlayerStateFactory.Instance.CreateState((int)_playerState);
        state.PlayState(transform, _playerRigidBody2d);
        CheckStop();
    }

    void CheckStop(){
        if(PlayerStatus.Instance.Speed <= 0 && _playerState == EPlayerState.RunStop){
            PlayerStatus.Instance.direction = Vector2.zero;
            PlayerStatus.Instance.Speed  = 0;
            _playerState = EPlayerState.Idle;
        }
    }

    protected override void SetInstance()=>Instance = this;
}

public class PlayerStatus : Singleton<PlayerStatus>{
    public float MaxSpeed = 10;
    public float Speed = 0;
    public float Jump = 10;
    public Vector2 direction;
}
