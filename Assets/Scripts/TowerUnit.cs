using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerUnit : Unit
{
    [SerializeField] Animator animator;
    const float AttackRange = 4;

    const float CooldownDuration = 3f;
    float cooldownTimer;

    private Level level;

    protected override void Update()
    {
        base.Update();

        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
            return;
        }

        var nearestEnemy = level.GetNearestEnemy(transform.position);
        if (nearestEnemy != null && Vector3.Distance(transform.position, nearestEnemy.transform.position) <= AttackRange)
        {
            Attack(nearestEnemy);
        }
    }

    internal void Activate()
    {
        gameObject.SetActive(true);
    }

    void Attack(EnemyUnit nearestEnemy)
    {
        cooldownTimer = CooldownDuration;
        animator.SetTrigger("Attack");
        nearestEnemy.Die();
    }

    internal void SetLevel(Level level)
    {
        this.level = level;
    }
}
