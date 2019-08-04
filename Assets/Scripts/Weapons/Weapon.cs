using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{

    public readonly SelectionType[] validTargets;
    public GameObject bulletPrefab;
    public Transform muzzle;
    public virtual int damage;
    public float fireCooldown;
    public float bulletVelocity;
    private int lastFired;

    void Start()
    {

    }

    void Update()
    {

    }

    virtual void Fire(GameObject target)
    {
        if (Time.time < lastFired + fireCooldown) return;

        Unit unit = target.GetComponent<Unit>();
        if (unit == null || !Array.Exists(validTargets, ty => ty == unit.SelectionType)) return;

        GameObject bullet = Instantiate(bulletPrefab, muzzle.position, Quaternion.identity);

        Vector3 dr = target.GetComponent<Transform>().position - muzzle.position;

        // Compensate for gravity to make sure we can actually hit the target
        float dt = dr.magnitude / bulletVelocity;
        float dropoff = bullet.GetComponent<Rigidbody>().useGravity ? dt * Physics.gravity : 0;

        bullet.GetComponent<Rigidbody>().velocity = dr.normalized * bulletVelocity - dropoff / dt;
        bullet.GetComponent<Bullet>().damage = damage;
        bullet.GetComponent<Bullet>().shooter = gameObject;
    }
}