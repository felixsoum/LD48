using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] TowerUnit towerUnit;

    [SerializeField] GameObject tileTop;
    [SerializeField] GameObject tileStart;
    [SerializeField] GameObject tileEnd;
    private Level level;
    internal Action OnTowerPlaced;

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

    private void OnMouseDown()
    {
        if (tileTop.activeInHierarchy && !towerUnit.gameObject.activeInHierarchy)
        {
            OnTowerPlaced?.Invoke();
            towerUnit.Activate();
        }
    }

    internal void SetLevel(Level level)
    {
        this.level = level;
        towerUnit.SetLevel(level);
    }
}
