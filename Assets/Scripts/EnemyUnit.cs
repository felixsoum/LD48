using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnit : Unit
{
    private const int MoveSpeed = 2;
    List<Vector3> waypoints = new List<Vector3>();
    public Action<EnemyUnit> OnDeath;

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

    internal void Die()
    {
        OnDeath?.Invoke(this);
        Destroy(gameObject);
    }
}
