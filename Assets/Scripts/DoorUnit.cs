using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorUnit : Unit
{
    [SerializeField] GameObject sphere;

    internal void Activate()
    {
        gameObject.SetActive(true);
    }

    internal void Reset()
    {
        gameObject.SetActive(false);
    }

    internal void HideSphere()
    {
        sphere.gameObject.SetActive(false);
    }

    internal void ShowSphere()
    {
        sphere.gameObject.SetActive(true);
    }
}
