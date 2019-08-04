using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Will be set by the Weapon when shooting
    public int damage;
    public GameObject shooter;
    public SelectionType intendedTarget => shooter.GetComponent<Unit>().SelectionType;
    
    void OnCollisionEnter(Collision other)
    {
        GameObject target = other.gameObject;
        Unit targetUnit = target.GetComponent<Unit>();
        Life targetLife = target.GetComponent<Life>();

        Destroy(gameObject);

        if (targetUnit != null && targetUnit.SelectionType == intendedTarget && targetLife != null)
        {
            targetLife.Damage(damage);

            SlimeWeapon weapon = shooter.GetComponent<SlimeWeapon>();
            if (weapon != null && Random.Range(0, 1) < weapon.BlobSpawnChance)
            {
                GameObject child = Instantiate(shooter, GetComponent<Transform>().position, Quaternion.identity);
                child.GetComponent<Life>().HP = weapon.LifeCost;
            }
        }
    }
}