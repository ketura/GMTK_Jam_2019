using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{

    public IEnumerable<SelectionType> validTargets;
    public GameObject bulletPrefab;
    public Transform muzzle;
    public int damage;
    public float fireCooldown;
    public float bulletVelocity;
    private int lastFired;

    public void Start()
    {

    }

    void Update()
    {

    }

    public virtual int Damage()
    {
        return damage;
    }

    public virtual void Fire(GameObject target)
    {
        if (Time.time < lastFired + fireCooldown) return;

        Unit unit = target.GetComponent<Unit>();
        if (unit == null || validTargets.Contains(unit.SelectionType)) return;

        GameObject bullet = Instantiate(bulletPrefab, muzzle.position, Quaternion.identity);

        Vector3 dr = target.GetComponent<Transform>().position - muzzle.position;
        bullet.GetComponent<Rigidbody>().velocity = dr.normalized * bulletVelocity;

        // Compensate for gravity to make sure we can actually hit the target
        if (bullet.GetComponent<Rigidbody>().useGravity)
        {
            float dt = dr.magnitude / bulletVelocity;
            Vector3 dropoff = Physics.gravity * dt;
            bullet.GetComponent<Rigidbody>().velocity -= dropoff / dt;
        }

        bullet.GetComponent<Bullet>().damage = Damage();
        bullet.GetComponent<Bullet>().shooter = GetComponent<Unit>();
    }
}