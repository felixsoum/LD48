using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] GameObject visualObject;

    Camera playerCamera;
    float currentScale = 1;
    float hitScale = 1.25f;

    float orientation = 1;

    protected virtual void Awake()
    {
        playerCamera = Camera.main;
    }

    protected virtual void Update()
    {
        currentScale = Mathf.Lerp(currentScale, 1, Time.deltaTime * 10);
        UpdateVisual();
        visualObject.transform.forward = playerCamera.transform.forward;
    }

    private void UpdateVisual()
    {
        visualObject.transform.localScale = new Vector3(currentScale * orientation, currentScale, currentScale);
    }

    protected void SetOrientation(bool isFacingRight)
    {
        orientation = isFacingRight ? 1 : -1;
        UpdateVisual();
    }

    internal virtual void Damage()
    {
        currentScale = hitScale;
        UpdateVisual();
    }
}
