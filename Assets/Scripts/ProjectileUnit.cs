using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileUnit : Unit
{
    private const int BaseSpeed = 10;
    private int depth;

    internal Unit TargetUnit { get; set; }

    protected override void Update()
    {
        base.Update();
        if (TargetUnit != null)
        {

            float moveDelta = BaseSpeed * Time.deltaTime * Level.GetDepthFactor(depth);
            var nextPosition = Vector3.MoveTowards(transform.position, TargetUnit.transform.position, moveDelta);
            float distanceThreshold = 0.01f * Level.GetDepthFactor(depth);
            if (Vector3.Distance(nextPosition, TargetUnit.transform.position) < distanceThreshold)
            {
                TargetUnit.Damage();
                Die();
            }
            else
            {
                transform.position = nextPosition;
            }
        }
        else
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    internal void SetDepth(int depth)
    {
        this.depth = depth;
        transform.localScale = Vector3.one * Level.GetDepthFactor(depth);
    }
}
