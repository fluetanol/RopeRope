using UnityEngine;
using UnityEngine.InputSystem;

public interface IState{
    public void PlayState(Transform transform, Rigidbody2D rigidbody2D);
}


public interface IInputState{
    public void PlayInputState(InputAction.CallbackContext context, Transform transform);
}

public interface IConditionState{
    public void PlayConditionState(Transform transform, Rigidbody2D rigidbody2D);
}