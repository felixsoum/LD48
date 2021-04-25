using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    private const int CameraSpeed = 10;

    public Transform TargetTransform { get; internal set; }

    private void Update()
    {
        if (TargetTransform != null)
        {
            transform.position = Vector3.Lerp(transform.position, TargetTransform.position, 2 * Time.deltaTime);
        }
    }

    internal void Move(float horizontalDelta, float verticalDelta)
    {
        transform.position += new Vector3(horizontalDelta, 0, verticalDelta) * Time.deltaTime * CameraSpeed;
    }
}
