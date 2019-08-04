using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Will be set by the Weapon when shooting
    public int damage;
    public Unit shooter;
    public SelectionType intendedTarget => shooter.SelectionType;
    
    void OnCollisionEnter(Collision other)
    {
        GameObject target = other.gameObject;
        Unit targetUnit = target.GetComponent<Unit>();
        Life targetLife = target.GetComponent<Life>();
        if (targetUnit != null && targetUnit.SelectionType == intendedTarget && targetLife != null)
        {
            targetLife.Damage(damage);
        }
        Destroy(gameObject);
    }
}