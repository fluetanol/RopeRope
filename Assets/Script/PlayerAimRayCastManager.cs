using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;


public interface IMouseClick
{
    public void OnClick(InputAction.CallbackContext context);
}

public class PlayerAimRayCastManager : MonoBehaviour, IMouseClick
{
    private LineRenderer _lineRenderer;
    private Vector3 _aimPosition;
    private Vector2 _playerClickPosition;
    private bool isClick = false;
    private float _distance = 30;

    public Transform debugCircle;
    Vector2 hitPoint;

    void Start(){
        isClick = false;
        _lineRenderer = GetComponent<LineRenderer>();
    }
    
    // Update is called once per frame
    void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, _aimPosition, _distance, LayerMask.GetMask("Default"));
        if(!isClick)LineRenderderControl(hit);
        ClickControl(hit);
        
    }



    public void OnClick(InputAction.CallbackContext context)
    {
        if(context.started){
            ResetRope();
            isClick = true;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, _aimPosition, _distance, LayerMask.GetMask("Default"));
            hitPoint = hit.point;
            _aimPosition = (Vector3)hit.point - transform.position;
            _playerClickPosition = Vector2.zero;

        }
    }

    private void ClickControl(RaycastHit2D hit){
        if (isClick) {
            _playerClickPosition += (Vector2)_aimPosition / 20;
            _lineRenderer.SetPosition(1, _playerClickPosition);

            if (_playerClickPosition.magnitude > _aimPosition.magnitude)
            {
                debugCircle.position = hitPoint;
                _lineRenderer.SetPosition(1, (Vector3)hitPoint - transform.position);
                PlayerStateManager.Instance.PlayerConditionState = EPlayerConditionState.Rope;
                PlayerStateManager.Instance.PlayerInputState = EPlayerInputState.RopeMove;
                PlayerInputManager.Instance.SetInputMap(1);
                // isClick = false;
            }
        }
        else{
            _aimPosition = (Vector3)CursorManager.Instance.GetCursorPosition() - this.transform.position;
        }
    }


    private void LineRenderderControl(RaycastHit2D hit){
        if (!hit.transform.IsUnityNull() && !_lineRenderer.enabled && hit.normal != Vector2.up)
        {
            _lineRenderer.enabled = true;
            debugCircle.gameObject.SetActive(true);
        }
        else if ((hit.transform.IsUnityNull() || hit.normal == Vector2.up) && _lineRenderer.enabled)
        {
            _lineRenderer.enabled = false;
            debugCircle.gameObject.SetActive(false);
        }
        else
        {
            debugCircle.position = hit.point;
            _lineRenderer.SetPosition(1, _aimPosition);
        }
    }

    private void ResetRope(){
        _lineRenderer.SetPosition(1, Vector2.zero);
        isClick = false;
    }
}

