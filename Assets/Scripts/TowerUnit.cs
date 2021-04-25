using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerUnit : Unit
{
    [SerializeField] Animator animator;
    [SerializeField] ProjectileUnit projectilePrefab;
    const float AttackRange = 4;

    const float CooldownDuration = 1f;
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
        cooldownTimer = CooldownDuration;
        gameObject.SetActive(true);
    }

    void Attack(EnemyUnit nearestEnemy)
    {
        SetOrientation(transform.position.x < nearestEnemy.transform.position.x);
        cooldownTimer = CooldownDuration;
        var projectilePosition = transform.position + Vector3.up * 0.5f * Level.GetDepthFactor(level.Depth);
        var projectileObject = Instantiate(projectilePrefab, projectilePosition, Quaternion.identity);
        ProjectileUnit projectileUnit = projectileObject.GetComponent<ProjectileUnit>();
        projectileUnit.SetDepth(level.Depth);
        projectileUnit.TargetUnit = nearestEnemy;
        
        animator.SetTrigger("Attack");
    }

    internal void SetLevel(Level level)
    {
        this.level = level;
    }

    internal void Reset()
    {
        gameObject.SetActive(false);
    }
}
