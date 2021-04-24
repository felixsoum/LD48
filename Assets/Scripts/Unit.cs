using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] GameObject visualObject;

    Camera playerCamera;

    private void Awake()
    {
        playerCamera = Camera.main;
    }

    protected virtual void Update()
    {
        visualObject.transform.forward = playerCamera.transform.forward;
    }

    protected void SetOrientation(bool isFacingRight)
    {
        visualObject.transform.localScale = new Vector3(isFacingRight ? 1 : -1, 1, 1);
    }
}
