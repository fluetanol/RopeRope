using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;


public interface IMouseClick
{
    public void OnClick(InputAction.CallbackContext context);
}

public class PlayerRopeInputManager : MonoBehaviour, IMouseClick
{
    private LineRenderer _lineRenderer;
    private Rigidbody2D _rigidbody2D;
    private bool isClick = false;
    private Vector2 _ropeHitPosition;
    private Vector2 _ropeHitNormal;

    public float _minRopeDistance = 5;
    public float _launchSpeed = 5;
    public float Velocity;
    public float damping = 0.99f;
    private float angle;
    private float cooldown = -1;

    void Start(){
        isClick = false;
        _lineRenderer = GetComponent<LineRenderer>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        Velocity = 3f;
    }
    
    // Update is called once per frame
    private void FixedUpdate() {
        _lineRenderer.SetPosition(0, transform.position);
        if (isClick && PlayerStateManager.Instance.PlayerInputState == EPlayerInputState.Launch)RopeGrabMove();
        else if (PlayerStateManager.Instance.PlayerInputState == EPlayerInputState.RopeMove) RopeMove();
        else  _lineRenderer.SetPosition(1, transform.position);

        if(cooldown >= 0) {
            cooldown+=Time.fixedDeltaTime;
            if(cooldown >= 0.75f) cooldown = -1;
        }
        
    }

    private void RopeGrabMove(){
        PlayerStatus.CurrentPos = _rigidbody2D.position;
        PlayerStatus.NextPos += (_ropeHitPosition - PlayerStatus.CurrentPos) * (Time.fixedDeltaTime * _launchSpeed);
        _rigidbody2D.MovePosition(PlayerStatus.NextPos);

        float distance = Vector2.Distance(_ropeHitPosition, _rigidbody2D.position);

        if (distance <= _minRopeDistance && _ropeHitNormal == Vector2.down){
            isClick = false;
            PlayerStateManager.Instance.PlayerInputState = EPlayerInputState.RopeMove;
            print(PlayerStatus.CurrentPos - _ropeHitPosition);
            angle = -Vector2.Angle(PlayerStatus.CurrentPos - _ropeHitPosition, Vector2.down) * Mathf.Deg2Rad;

            angle  = (PlayerStatus.CurrentPos - _ropeHitPosition).x < 0 ? angle : angle*-1;

        }
        else if(distance <= 1){
            isClick = false;
            PlayerStateManager.Instance.PlayerConditionState = EPlayerConditionState.Air;
            PlayerStateManager.Instance.PlayerInputState = EPlayerInputState.Jump;
            // PlayerStateManager.Instance.PlayerInputState = EPlayerInputState.Stop;
        }
    }

    private void RopeMove(){
        PlayerStatus.CurrentPos = _rigidbody2D.position;
        float dt = Time.fixedDeltaTime;
        float acceleration = -10f / _minRopeDistance * Mathf.Sin(angle);

        // 새로운 각 속도 계산 (오일러 적분)
        Velocity += acceleration * dt;
        // 감쇠 적용
        Velocity *= damping;

        // 새로운 각도 계산
        angle += Velocity * dt;

        // 진자의 위치 갱신
        float x = _ropeHitPosition.x + _minRopeDistance * Mathf.Sin(angle);
        float y = _ropeHitPosition.y - _minRopeDistance * Mathf.Cos(angle);
        PlayerStatus.NextPos = new Vector3(x,y,0);
        _rigidbody2D.MovePosition(PlayerStatus.NextPos);
    }


    public void OnClick(InputAction.CallbackContext context)
    {
        if(context.started && CursorManager.Instance.isCursorHit() && cooldown == -1){
            cooldown = 0;
            _lineRenderer.SetPosition(1, CursorManager.Instance.GetHitPosition());
            Velocity = 3f;
            isClick = true;
            _ropeHitPosition = CursorManager.Instance.GetHitPosition();
            _ropeHitNormal = CursorManager.Instance.GetHitNormal();
            PlayerStateManager.Instance.PlayerConditionState = EPlayerConditionState.RopeIdle;
            PlayerStateManager.Instance.PlayerInputState = EPlayerInputState.Launch;

        }
    }
}

