using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Camera TargetCamera;
    public Transform TargetPosition;
    public float CameraMoveSmooth;

    private Transform CameraTransform;

    private void Start()
    {
        CameraTransform = TargetCamera.transform;
    }

    private void Update()
    {
        Vector2 targetPosition = TargetPosition.position;
        targetPosition = Vector2.Lerp(CameraTransform.position, targetPosition, CameraMoveSmooth * Time.deltaTime);
        CameraTransform.position = new Vector3(targetPosition.x, targetPosition.y, CameraTransform.position.z);
    }
}
