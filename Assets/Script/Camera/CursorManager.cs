using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public interface IMouseCursorAim{
    public void OnMouseAim(InputAction.CallbackContext callback);
    public Vector2 GetCursorPosition();
}

public class CursorManager : SingletonMonobehavior<CursorManager>, IMouseCursorAim
{
    public Transform _hitCursor;
    public Transform _playerTransform;
    public float _rayDistance = 20f;

    private Vector3 mousePos;
    private Vector2 worldPos;
    private Vector2 _hitNormal;
    private bool _isHit;

    public void OnMouseAim(InputAction.CallbackContext callback)
    {
        mousePos = callback.ReadValue<Vector2>();
        worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        mousePos.z = 0;
        this.transform.position = worldPos;

        Vector2 playerPos = _playerTransform.position;
        Vector2 rayDirection = worldPos - playerPos;
        RaycastHit2D hit = Physics2D.Raycast(playerPos, rayDirection, _rayDistance, LayerMask.GetMask("Default"));
    
        if(!hit.transform.IsUnityNull()){
            if(_hitCursor.gameObject.activeSelf == false) _hitCursor.gameObject.SetActive(true);
            _hitCursor.position = hit.point;
            _hitNormal = hit.normal;
            _isHit = true;
        }
        else {
            _hitCursor.gameObject.SetActive(false);
            _isHit = false;
        }
    }


    public Vector2 GetCursorPosition() => this.transform.position;
    public Vector2 GetHitPosition() => _hitCursor.position;
    public Vector2 GetHitNormal() => _hitNormal;
    public bool isCursorHit() => _isHit;

    protected override void SetInstance(){
        Instance = this;
    }
}
