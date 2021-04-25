using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelNode
{
    public int x;
    public int y;

    public LevelNode(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
}

public class Level : MonoBehaviour
{
    [SerializeField] GameDirector gameDirector;
    [SerializeField] GameObject tilePrefab;
    [SerializeField] Transform tileParent;
    [SerializeField] Tile[] tiles;
    [SerializeField] GameObject enemyPrefab;

    public Transform cameraPosition;
    internal int Depth { get; set; }
    const int RowCount = 8;
    const int ColCount = 8;
    HashSet<EnemyUnit> enemies = new HashSet<EnemyUnit>();
    int waveNumber = 1;
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

    char[,] scriptedLevel2 = new[,]
    {
        { '#', '#', '#', '#', '#', '#', '#', '#' },
        { '#', '#', '#', '#', '#', '#', 'E', '#' },
        { '#', '_', '_', '_', '_', '_', '_', '#' },
        { '#', '_', '#', '#', '#', '#', '#', '#' },
        { '#', '_', '_', '_', '_', '_', '_', '#' },
        { '#', '#', '#', '#', '#', '#', '_', '#' },
        { '#', 'S', '_', '_', '_', '_', '_', '#' },
        { '#', '#', '#', '#', '#', '#', '#', '#' },
    };

    LevelNode[] levelPath = new[]
    {
        new LevelNode(2, 6),
        new LevelNode(3, 6),
        new LevelNode(4, 6),
        new LevelNode(5, 6),
        new LevelNode(6, 6),
        new LevelNode(6, 5),
        new LevelNode(6, 4),
        new LevelNode(6, 3),
        new LevelNode(6, 2),
        new LevelNode(6, 1),
        new LevelNode(5, 1),
        new LevelNode(4, 1),
        new LevelNode(3, 1),
        new LevelNode(2, 1),
        new LevelNode(2, 2),
        new LevelNode(2, 3),
        new LevelNode(2, 4),
        new LevelNode(3, 4),
        new LevelNode(4, 4),
    };

    LevelNode[] levelPath2 = new[]
{
        new LevelNode(2, 1),
        new LevelNode(3, 1),
        new LevelNode(4, 1),
        new LevelNode(5, 1),
        new LevelNode(6, 1),
        new LevelNode(6, 2),
        new LevelNode(6, 3),
        new LevelNode(5, 3),
        new LevelNode(4, 3),
        new LevelNode(3, 3),
        new LevelNode(2, 3),
        new LevelNode(1, 3),
        new LevelNode(1, 4),
        new LevelNode(1, 5),
        new LevelNode(2, 5),
        new LevelNode(3, 5),
        new LevelNode(4, 5),
        new LevelNode(5, 5),
        new LevelNode(6, 5),
        new LevelNode(6, 5),
    };

    private int startIndex;
    private int endIndex;
    private LevelNode[] chosenPath;

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

    internal void EnableColliders()
    {
        foreach (var tile in tiles)
        {
            tile.EnableColliders();
        }
    }

    internal void DisableColliders()
    {
        foreach (var tile in tiles)
        {
            tile.DisableColliders();
        }
    }

    private void Start()
    {
        InitTiles();

        if (Depth == 0)
        {
            StartCoroutine(SpawnEnemies()); 
        }
    }

    private IEnumerator SpawnEnemies()
    {
        yield return new WaitUntil(() => towerPlacedCount > 0);
        yield return new WaitForSeconds(0.5f);
        while (true)
        {
            int spawnCount = waveNumber * 2;
            for (int i = 0; i < spawnCount; i++)
            {
                SpawnEnemy();
                yield return new WaitForSeconds(3f / waveNumber);
            }
            float longerWait = 3f + Mathf.Sqrt(waveNumber * 3);
            yield return new WaitForSeconds(longerWait);
            waveNumber++;
        }
    }

    internal void SpawnEnemy(EnemyUnit enemyToClone = null)
    {
        var spawnPosition = tiles[startIndex].transform.position;
        var enemyObject = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        var enemyUnit = enemyObject.GetComponent<EnemyUnit>();

        enemyObject.transform.localScale = transform.localScale;
        enemyUnit.SetGameDirector(gameDirector);
        enemyUnit.Depth = Depth;
        if (enemyToClone == null)
        {
            enemyUnit.SetWaveNumber(waveNumber); 
        }
        else
        {
            enemyUnit.Copy(enemyToClone);
        }
        enemyUnit.OnDeath += deadEnemy => enemies.Remove(deadEnemy);
        enemies.Add(enemyUnit);

        InitEnemy(enemyUnit);
    }

    private void InitEnemy(EnemyUnit enemyUnit)
    {
        foreach (var tileNode in chosenPath)
        {
            enemyUnit.AddWaypoint(tiles[tileNode.x + tileNode.y * RowCount]);

        }
        //enemyUnit.AddWaypoint(tiles[4 + 4 * RowCount].transform.position);
        enemyUnit.AddWaypoint(tiles[endIndex]);
    }

    private void InitTiles()
    {
        bool isFirstLevel = UnityEngine.Random.value > 0.5f;
        var chosenLevel = isFirstLevel ? scriptedLevel : scriptedLevel2;
        chosenPath = isFirstLevel ? levelPath : levelPath2;

        for (int y = 0; y < RowCount; y++)
        {
            for (int x = 0; x < ColCount; x++)
            {
                char tileType = chosenLevel[RowCount - 1 - y, x];
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
                tile.Reset();
                tile.Depth = Depth;
                tile.SetLevel(this);
                tile.OnTowerPlaced += () => towerPlacedCount++;
                tile.OnLevelZoom += OnLevelZoom;
                tile.SetTile(tileType);
            }
        }
    }

    private void OnLevelZoom(Level innerLevel)
    {
        foreach (var tile in tiles)
        {
            tile.DisableColliders();
        }
        gameDirector.OnLevelZoom(innerLevel);
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

    internal static float GetDepthFactor(int depth) => Mathf.Pow(0.1f, depth);
}
