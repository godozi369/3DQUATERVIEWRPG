using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;    // 따라갈 대상 
    public Vector3 offset = new Vector3(0, 10, -7); // 기본 거리 
    private Quaternion initialRotation; // 처음 회전 값 저장 

    private void Start()
    {
        // 시작할 때 카메라 회전 고정
        initialRotation = transform.rotation;
    }

    private void LateUpdate()
    {
        if (target != null)
        {
            transform.position = target.position + offset;
            transform.rotation = initialRotation; // 회전 고정 
        }
    }
}
