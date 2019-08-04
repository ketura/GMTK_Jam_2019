using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Life))]
public class SlimeWeapon : Weapon
{
    public int LifeCost;
    public int DamagePerLife;
    public override int damage => (GetComponent<Life>().HP + DamagePerLife - 1) / DamagePerLife;
    public Life lifePool;

    void Start()
    {
        base.Start();
        if (lifePool == null)
        {
            lifePool = GetComponent<Life>();
        }
    }

    override void Fire(GameObject target)
    {
        if (lifePool.HP <= LifeCost) return;
        base.Fire(target);
        lifePool.Damage(LifeCost);
    }
}