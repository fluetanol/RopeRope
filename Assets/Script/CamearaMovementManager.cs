using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamearaMovementManager : MonoBehaviour
{
    public Transform Target;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 CurrentPosition =  this.transform.position;
        CurrentPosition.x = Target.position.x;
        CurrentPosition.y = Target.position.y;
        this.transform.position = CurrentPosition;
    }
}
