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
        // 注释部分为尝试平滑移动镜头的代码，因为有抖动，故不使用
        // Vector2 targetPosition = TargetPosition.position;
        // targetPosition = Vector2.Lerp(CameraTransform.position, targetPosition, CameraMoveSmooth * Time.deltaTime);
        // CameraTransform.position = new Vector3(targetPosition.x, targetPosition.y, CameraTransform.position.z);

        CameraTransform.position = new Vector3(TargetPosition.position.x, TargetPosition.position.y, CameraTransform.position.z);
    }
}
