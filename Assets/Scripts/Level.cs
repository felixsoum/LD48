using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] GameObject tilePrefab;
    [SerializeField] Transform tileParent;
    [SerializeField] Tile[] tiles;
    [SerializeField] GameObject enemyPrefab;
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
    private int startIndex;
    private int endIndex;

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
        InitTiles();

        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        yield return new WaitForSeconds(1);
        while (true)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(1);
        }
    }

    private void SpawnEnemy()
    {
        var spawnPosition = tiles[startIndex].transform.position;
        var enemyObject = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        var enemyUnit = enemyObject.GetComponent<EnemyUnit>();
        InitEnemy(enemyUnit);
    }

    private void InitEnemy(EnemyUnit enemyUnit)
    {
        enemyUnit.AddWaypoint(tiles[6 + 6 * RowCount].transform.position);
        enemyUnit.AddWaypoint(tiles[6 + 1 * RowCount].transform.position);
        enemyUnit.AddWaypoint(tiles[2 + 1 * RowCount].transform.position);
        enemyUnit.AddWaypoint(tiles[2 + 4 * RowCount].transform.position);
        enemyUnit.AddWaypoint(tiles[4 + 4 * RowCount].transform.position);
        enemyUnit.AddWaypoint(tiles[endIndex].transform.position);
    }

    private void InitTiles()
    {
        for (int y = 0; y < RowCount; y++)
        {
            for (int x = 0; x < ColCount; x++)
            {
                char tileType = scriptedLevel[RowCount - 1 - y, x];
                int tileIndex = x + y * RowCount;
                switch (tileType)
                {
                    case 'S':
                        startIndex = tileIndex;
                        break;
                    case 'E':
                        endIndex = tileIndex;
                        break;
                    default:
                        break;
                }
                tiles[tileIndex].SetTile(tileType);
            }
        }
    }
}
