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

    private void Update()
    {
        visualObject.transform.forward = playerCamera.transform.forward;
    }
}
