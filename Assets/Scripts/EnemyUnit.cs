using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnit : Unit
{
    [SerializeField] SphereCollider spherousCollider;
    [SerializeField] SpriteRenderer spriteRenderer;
    private const int BaseHp = 1;
    int currentHp = BaseHp;
    private const float BaseMoveSpeed = 0.25f;
    float currentMoveSpeed = BaseMoveSpeed;
    List<Tile> waypoints = new List<Tile>();
    public Action<EnemyUnit> OnDeath;
    private GameDirector gameDirector;
    Rigidbody myRigidbody;
    private bool isDead;

    public int Depth { get; internal set; }

    protected override void Awake()
    {
        base.Awake();
        myRigidbody = GetComponent<Rigidbody>();
    }

    protected override void Update()
    {
        base.Update();
        if (waypoints.Count > 0)
        {
            float distanceThreshold = 0.01f * Level.GetDepthFactor(Depth);
            Tile tile = waypoints[0];
            if (Vector3.Distance(transform.position, tile.transform.position) < distanceThreshold)
            {
                if (tile.IsDoorActive())
                {
                    tile.SpawnEnemy(this);
                    GoInsideDoor();
                }
                else
                {
                    waypoints.RemoveAt(0);
                }
            }
        }

        if (waypoints.Count > 0)
        {
            SetOrientation(transform.position.x < waypoints[0].transform.position.x);

            float moveDelta = currentMoveSpeed * Time.deltaTime * Level.GetDepthFactor(Depth);
            transform.position = Vector3.MoveTowards(transform.position, waypoints[0].transform.position, moveDelta);
        }
        else
        {
            Die();
        }
    }

    internal void AddWaypoint(Tile tile)
    {
        waypoints.Add(tile);
    }

    internal override void Damage()
    {
        base.Damage();
        if (--currentHp <= 0)
        {
            Die();
        }
    }

    internal void GoInsideDoor()
    {
        OnDeath?.Invoke(this);
        gameObject.SetActive(false);
    }

    internal void Die()
    {
        if (isDead)
        {
            return;
        }
        isDead = true;
        gameDirector.AddCoin(1);
        OnDeath?.Invoke(this);
        spherousCollider.enabled = true;
        myRigidbody.isKinematic = false;
        spriteRenderer.flipY = true;
        Vector3 ragdollForce = 0.5f * Vector3.up * Level.GetDepthFactor(Depth);
        ragdollForce += UnityEngine.Random.onUnitSphere * Level.GetDepthFactor(Depth) * 0.25f;
        myRigidbody.AddForce(ragdollForce, ForceMode.VelocityChange);
        Invoke("RemoveMe", 2f);
    }

    public void RemoveMe()
    {
        Destroy(gameObject);
    }

    internal void SetGameDirector(GameDirector gameDirector)
    {
        this.gameDirector = gameDirector;
    }

    internal void SetWaveNumber(int waveNumber)
    {
        currentHp += waveNumber;
        currentMoveSpeed *= waveNumber;
    }

    internal void Copy(EnemyUnit enemyToClone)
    {
        currentHp = enemyToClone.currentHp;
        currentMoveSpeed = enemyToClone.currentMoveSpeed;
    }
}
