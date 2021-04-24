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

    private void OnValidate()
    {
        if (tilePrefab == null)
        {
            return;
        }

        if (tiles == null || tiles.Length == 0)
        {
            tiles = new Tile[RowCount * ColCount];
            for (int y = 0; y < RowCount; y++)
            {
                for (int x = 0; x < ColCount; x++)
                {
                    var position = new Vector3(x, 0, y);
                    var tileObject = (GameObject) UnityEditor.PrefabUtility.InstantiatePrefab(tilePrefab, tileParent);
                    tileObject.transform.position = position;
                    tileObject.transform.rotation = Quaternion.identity;
                    tiles[x + y * RowCount] = tileObject.GetComponent<Tile>();
                }
            }
        }
    }
}
