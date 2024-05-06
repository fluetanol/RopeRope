using UnityEngine;
using UnityEngine.InputSystem;

public interface IMouseCursorAim{
    public void OnMouseAim(InputAction.CallbackContext callback);
    public Vector2 GetCursorPosition();
}

public class CursorManager : SingletonMonobehavior<CursorManager>, IMouseCursorAim
{
    private bool _cursorLock = true;

    public void OnMouseAim(InputAction.CallbackContext callback)
    {
        Vector3 mousePos = callback.ReadValue<Vector2>();
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        mousePos.z = 0;
        this.transform.position = worldPos;
        
    }

    public Vector2 GetCursorPosition() => this.transform.position;

    protected override void SetInstance(){
        Instance = this;
    }
}
