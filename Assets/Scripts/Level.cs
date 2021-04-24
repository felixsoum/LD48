using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] GameDirector gameDirector;
    [SerializeField] GameObject tilePrefab;
    [SerializeField] Transform tileParent;
    [SerializeField] Tile[] tiles;
    [SerializeField] GameObject enemyPrefab;
    const int RowCount = 8;
    const int ColCount = 8;
    HashSet<EnemyUnit> enemies = new HashSet<EnemyUnit>();
    int spawnCount = 1;

    int towerPlacedCount;
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
        yield return new WaitUntil(() => towerPlacedCount > 0);
        yield return new WaitForSeconds(0.5f);
        while (true)
        {
            for (int i = 0; i < spawnCount; i++)
            {
                SpawnEnemy();
                yield return new WaitForSeconds(2f / spawnCount);
            }
            float longerWait = 5f + Mathf.Sqrt(spawnCount);
            yield return new WaitForSeconds(longerWait);
            spawnCount *= 2;
        }
    }

    private void SpawnEnemy()
    {
        var spawnPosition = tiles[startIndex].transform.position;
        var enemyObject = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        var enemyUnit = enemyObject.GetComponent<EnemyUnit>();
        enemyUnit.SetGameDirector(gameDirector);
        enemyUnit.OnDeath += deadEnemy => enemies.Remove(deadEnemy);
        enemies.Add(enemyUnit);

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

                Tile tile = tiles[tileIndex];
                tile.SetLevel(this);
                tile.OnTowerPlaced += () => towerPlacedCount++;
                tile.SetTile(tileType);
            }
        }
    }

    internal EnemyUnit GetNearestEnemy(Vector3 position)
    {
        if (enemies.Count == 0)
        {
            return null;
        }

        EnemyUnit nearestEnemy = null;
        float nearestDistance = 0;

        foreach (var enemy in enemies)
        {
            float candidateDistance = Vector3.Distance(position, enemy.transform.position);

            if (nearestEnemy == null || candidateDistance < nearestDistance)
            {
                nearestEnemy = enemy;
                nearestDistance = candidateDistance;
            }
        }

        return nearestEnemy;
    }
}
