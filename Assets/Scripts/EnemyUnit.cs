using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnit : Unit
{
    private const int BaseHp = 5;
    int currentHp = BaseHp;
    private const float BaseMoveSpeed = 0.25f;
    float moveSpeed = BaseMoveSpeed;
    List<Vector3> waypoints = new List<Vector3>();
    public Action<EnemyUnit> OnDeath;
    private GameDirector gameDirector;

    public int Depth { get; internal set; }

    protected override void Update()
    {
        base.Update();
        if (waypoints.Count > 0)
        {
            float distanceThreshold = 0.01f * Level.GetDepthFactor(Depth);
            if (Vector3.Distance(transform.position, waypoints[0]) < distanceThreshold)
            {
                waypoints.RemoveAt(0);
            }
        }

        if (waypoints.Count > 0)
        {
            SetOrientation(transform.position.x < waypoints[0].x);

            float moveDelta = moveSpeed * Time.deltaTime * Level.GetDepthFactor(Depth);
            transform.position = Vector3.MoveTowards(transform.position, waypoints[0], moveDelta);
        }
        else
        {
            Die();
        }
    }

    internal void AddWaypoint(Vector3 position)
    {
        waypoints.Add(position);
    }

    internal override void Damage()
    {
        base.Damage();
        if (--currentHp <= 0)
        {
            Die();
        }
    }

    internal void Die()
    {
        gameDirector.AddCoin(1);
        OnDeath?.Invoke(this);
        Destroy(gameObject);
    }

    internal void SetGameDirector(GameDirector gameDirector)
    {
        this.gameDirector = gameDirector;
    }

    internal void SetWaveNumber(int waveNumber)
    {
        currentHp += waveNumber;
        moveSpeed *= waveNumber;
    }
}
