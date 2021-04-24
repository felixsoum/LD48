using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    private const int CameraSpeed = 10;

    private void Update()
    {
        float horizontalDelta = Input.GetAxisRaw("Horizontal");
        float verticalDelta = Input.GetAxisRaw("Vertical");

        transform.position += new Vector3(horizontalDelta, 0, verticalDelta) * Time.deltaTime * CameraSpeed;
    }
}
