using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Life))]
public class SlimeWeapon : Weapon
{
    public int LifeCost;
    public float DamagePerLife;
    public Life lifePool;

    void Start()
    {
        base.Start();
        if (lifePool == null)
        {
            lifePool = GetComponent<Life>();
        }
    }

    public override int Damage()
    {
        return (int) Math.Floor((float) GetComponent<Life>().HP / DamagePerLife);
    }

    public override void Fire(GameObject target)
    {
        if (lifePool.HP <= LifeCost) return;
        base.Fire(target);
        lifePool.Damage(LifeCost);
    }
}