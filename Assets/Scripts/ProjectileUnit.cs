using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileUnit : Unit
{
    private const int Speed = 10;
    internal Unit TargetUnit { get; set; }

    protected override void Update()
    {
        base.Update();
        if (TargetUnit != null)
        {

            var nextPosition = Vector3.MoveTowards(transform.position, TargetUnit.transform.position, Speed * Time.deltaTime);
            if (Vector3.Distance(nextPosition, TargetUnit.transform.position) < 0.01f)
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
}
