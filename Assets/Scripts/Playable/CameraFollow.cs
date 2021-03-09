using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Camera TargetCamera;
    public Transform TargetPosition;
    public Vector2 Offset;
    private void Update()
    {
        Vector3 targetPosition = TargetPosition.position;
        targetPosition.x += Offset.x;
        targetPosition.y += Offset.y;
        targetPosition.z = TargetCamera.transform.position.z;
        TargetCamera.gameObject.transform.position = targetPosition;
    }
}
