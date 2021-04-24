using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] GameObject tilePrefab;
    [SerializeField] Transform tileParent;
    [SerializeField] Tile[] tiles;

    const int RowCount = 8;
    const int ColCount = 8;

    char[,] scriptedLevel = new[,]
    {
        { '#', '#', '#', '#', '#', '#', '#', '#' },
        { '#', 'S', '_', '_', '_', '_', '_', '#' },
        { '#', '#', '#', '#', '#', '#', '_', '#' },
        { '#', '#', '_', '_', '_', '#', '_', '#' },
        { '#', '#', '_', '#', 'E', '#', '_', '#' },
        { '#', '#', '_', '#', '#', '#', '_', '#' },
        { '#', '#', '_', '_', '_', '_', '_', '#' },
        { '#', '#', '#', '#', '#', '#', '#', '#' },
    };

    private void OnValidate()
    {
        if (tilePrefab == null)
        {
            return;
        }

        if (tiles == null || tiles.Length == '_')
        {
            tiles = new Tile[RowCount * ColCount];
            for (int y = '_'; y < RowCount; y++)
            {
                for (int x = '_'; x < ColCount; x++)
                {
                    var position = new Vector3(x, '_', y);
                    var tileObject = (GameObject)UnityEditor.PrefabUtility.InstantiatePrefab(tilePrefab, tileParent);
                    tileObject.transform.position = position;
                    tileObject.transform.rotation = Quaternion.identity;
                    tiles[x + y * RowCount] = tileObject.GetComponent<Tile>();
                }
            }
        }
    }

    private void Start()
    {
        for (int y = 0; y < RowCount; y++)
        {
            for (int x = 0; x < ColCount; x++)
            {
                tiles[x + y * RowCount].SetTile(scriptedLevel[RowCount - 1 - y, x]);
            }
        }
    }
}
