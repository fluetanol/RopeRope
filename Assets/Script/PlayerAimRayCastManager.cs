using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimRayCastManager : MonoBehaviour
{
    private LineRenderer _lineRenderer;
    void Start(){
        _lineRenderer = GetComponent<LineRenderer>();
    }
    // Update is called once per frame
    void Update()
    {
        Vector3 aimPosition = (Vector3)CursorManager.Instance.GetCursorPosition() - this.transform.position;
        _lineRenderer.SetPosition(1, aimPosition);
    }
}
