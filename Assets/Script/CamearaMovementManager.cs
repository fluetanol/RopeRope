using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamearaMovementManager : MonoBehaviour
{
    public Transform Target;
    public float _cameraMoveSpeed;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        Vector3 currentPosition =  this.transform.position;
        currentPosition.x = Target.position.x;
        currentPosition.y = Target.position.y;
        Vector3 lerpPosition = Vector2.Lerp(transform.position, PlayerStatus.NextPos,
                                Time.deltaTime * _cameraMoveSpeed);

        
        lerpPosition.z = -10;
        transform.position = lerpPosition;


    }
}
