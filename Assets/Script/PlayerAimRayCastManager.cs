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
    private bool isClick = false;
    private float time;
    public float speed =3;

    public Transform debugCircle;
    Vector2 hitPoint;

    void Start(){
        _lineRenderer = GetComponent<LineRenderer>();
    }
    // Update is called once per frame
    void Update()
    {
        if(isClick) {
            time += Time.deltaTime;
            _lineRenderer.SetPosition(1, (_aimPosition.normalized)*time*speed);
        }
        else{
            _aimPosition = (Vector3)CursorManager.Instance.GetCursorPosition() - this.transform.position;
            _lineRenderer.SetPosition(1, _aimPosition);
        }

        if(time>0.5f){
            ResetRope();
        }
    }

    public void OnClick(InputAction.CallbackContext context)
    {
        ResetRope();
        isClick = true;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, _aimPosition, 100, LayerMask.GetMask("Default"));
        hitPoint = hit.point;
        debugCircle.position = hit.point;

        _aimPosition = (Vector3)CursorManager.Instance.GetCursorPosition() - this.transform.position;
    }

    private void ResetRope(){
        time = 0;
        _lineRenderer.SetPosition(1, Vector2.zero);
        isClick = false;
    }

}
