using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour, ISetPlayer
{
    public Camera TargetCamera;
    public Transform TargetPosition;
    public Vector2 Offset;
    private Transform CameraTransform;

    private void Start()
    {
        if(TargetCamera != null)
            CameraTransform = TargetCamera.transform;
    }

    private void Update()
    {
        // 注释部分为尝试平滑移动镜头的代码，因为有抖动，故不使用
        // Vector2 targetPosition = TargetPosition.position;
        // targetPosition = Vector2.Lerp(CameraTransform.position, targetPosition, CameraMoveSmooth * Time.deltaTime);
        // CameraTransform.position = new Vector3(targetPosition.x, targetPosition.y, CameraTransform.position.z);

        if(TargetPosition != null && CameraTransform != null)
            CameraTransform.position = new Vector3(TargetPosition.position.x + Offset.x, TargetPosition.position.y + Offset.y, CameraTransform.position.z);
    }

    public void SetPlayer(GameObject target)
    {
        TargetPosition = target.transform;
    }
}
