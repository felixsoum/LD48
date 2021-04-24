using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] GameObject tileTop;
    [SerializeField] GameObject tileStart;
    [SerializeField] GameObject tileEnd;

    internal void SetTile(char tileType)
    {
        switch(tileType)
        {
            case '#':
                tileTop.SetActive(true);
                break;
            case 'S':
                tileStart.SetActive(true);
                break;
            case 'E':
                tileEnd.SetActive(true);
                break;
        }
    }

}
