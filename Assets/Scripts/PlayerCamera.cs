using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    private const int CameraSpeed = 10;

    internal void Move(float horizontalDelta, float verticalDelta)
    {
        transform.position += new Vector3(horizontalDelta, 0, verticalDelta) * Time.deltaTime * CameraSpeed;
    }
}
