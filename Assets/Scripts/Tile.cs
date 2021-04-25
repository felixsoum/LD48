using System;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] GameObject levelPrefab;
    [SerializeField] TowerUnit towerUnit;
    [SerializeField] DoorUnit doorUnit;

    [SerializeField] GameObject tileTop;
    [SerializeField] GameObject tileStart;
    [SerializeField] GameObject tileEnd;

    [SerializeField] BoxCollider tileCollider;

    internal int Depth { get; set; }
    Level innerLevel;
    private Level level;
    Vector3 innerLevelOffset = new Vector3(-0.55f, -0.25f, -0.45f);
    internal Action OnTowerPlaced;
    internal Action<Level> OnLevelZoom;

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
        if (innerLevel != null)
        {
            doorUnit.HideSphere();
            OnLevelZoom?.Invoke(innerLevel);
        }
        else if (tileTop.activeInHierarchy)
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
            if (!doorUnit.gameObject.activeInHierarchy && !selectedShopUnit.isTower && Depth < 30)
            {
                doorUnit.Activate();
                Vector3 levelPosition = transform.position + GetInnerLevelOffset();
                var levelObject = Instantiate(levelPrefab, levelPosition, Quaternion.identity);
                innerLevel = levelObject.GetComponent<Level>();
                innerLevel.Depth = Depth + 1;
                levelObject.transform.localScale = Vector3.one * GetScale();
                return true;
            }
        }
        return false;
    }

    private float GetScale()
    {
        return Mathf.Pow(0.1f, Depth + 1);
    }

    Vector3 GetInnerLevelOffset()
    {
        return innerLevelOffset * Mathf.Pow(0.1f, Depth);
    }

    internal void SetLevel(Level level)
    {
        this.level = level;
        towerUnit.SetLevel(level);
    }

    internal void EnableColliders()
    {
        tileCollider.enabled = true;
        doorUnit.ShowSphere();
    }

    internal void DisableColliders()
    {
        tileCollider.enabled = false;
        doorUnit.HideSphere();
    }

    internal void Reset()
    {
        towerUnit.Reset();
        doorUnit.Reset();
    }
}
