using System;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] TowerUnit towerUnit;
    [SerializeField] DoorUnit doorUnit;

    [SerializeField] GameObject tileTop;
    [SerializeField] GameObject tileStart;
    [SerializeField] GameObject tileEnd;
    private Level level;
    internal Action OnTowerPlaced;

    internal void SetTile(char tileType)
    {
        switch (tileType)
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

    internal bool OnClick(UnitBuyButton selectedShopUnit)
    {
        if (tileTop.activeInHierarchy)
        {
            if (!towerUnit.gameObject.activeInHierarchy && selectedShopUnit.isTower)
            {
                OnTowerPlaced?.Invoke();
                towerUnit.Activate();
                return true;
            }
        }
        else
        {
            if (!doorUnit.gameObject.activeInHierarchy && !selectedShopUnit.isTower)
            {
                doorUnit.Activate();
                return true;
            }
        }
        return false;
    }

    internal void SetLevel(Level level)
    {
        this.level = level;
        towerUnit.SetLevel(level);
    }
}
