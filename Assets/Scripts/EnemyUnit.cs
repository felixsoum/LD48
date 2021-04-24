using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnit : Unit
{
    int hp = 10;
    private const float MoveSpeed = 0.5f;
    List<Vector3> waypoints = new List<Vector3>();
    public Action<EnemyUnit> OnDeath;
    private GameDirector gameDirector;

    protected override void Update()
    {
        base.Update();
        if (waypoints.Count > 0)
        {
            if (Vector3.Distance(transform.position, waypoints[0]) < 0.01f)
            {
                waypoints.RemoveAt(0);
            }
        }

        if (waypoints.Count > 0)
        {
            SetOrientation(transform.position.x < waypoints[0].x);

            transform.position = Vector3.MoveTowards(transform.position, waypoints[0], MoveSpeed * Time.deltaTime);
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
        if (--hp <= 0)
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
}
