using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] GameObject tileTop;
    [SerializeField] GameObject tileStart;
    [SerializeField] GameObject tileEnd;

    private void Awake()
    {
        if (UnityEngine.Random.value > 0.5f)
        {
            if (Random.value > 0.5f)
            {
                tileTop.SetActive(true);
            }
            else if (Random.value > 0.1f)
            {
                if (Random.value > 0.5f)
                {
                    tileStart.SetActive(true);
                }
                else
                {
                    tileEnd.SetActive(true);
                }
            }
        }
    }
}
