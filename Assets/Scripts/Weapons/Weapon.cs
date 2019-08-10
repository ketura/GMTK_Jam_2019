using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    // public SelectionType[] validTargets;
    public GameObject bulletPrefab;
    public Transform muzzle;
    public int damage;
    public float fireCooldown;
    public float bulletVelocity;
    private float lastFired;
    public bool CanFire => Time.time > lastFired + fireCooldown;

	public AudioClip ShootSound;

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
        // print($"{gameObject} attempting to fire; can = {CanFire}");
        if (!CanFire) return;

        Unit unit = target.GetComponent<Unit>();
		target.GetComponentInParent<Life>().Damage(damage);
		AudioManager.Instance.PlayClip(ShootSound);
		lastFired = Time.time;
		
		// if (unit == null || !validTargets.Contains(unit.SelectionType)) return;

		GameObject bullet = Instantiate(bulletPrefab, muzzle.position, Quaternion.identity);

        Vector3 dr = target.GetComponent<Transform>().position - muzzle.position;
        bullet.GetComponent<Rigidbody>().velocity = dr.normalized * bulletVelocity;

        // Compensate for gravity to make sure we can actually hit the target
        if (bullet.GetComponent<Rigidbody>().useGravity)
        {
            float dt = dr.magnitude / bulletVelocity;
            Vector3 dropoff = Physics.gravity * dt*dt / 2;
            bullet.GetComponent<Rigidbody>().velocity -= dropoff / dt;
        }

		bullet.GetComponent<Bullet>().damage = 0;//Damage();
        bullet.GetComponent<Bullet>().shooter = gameObject;

        // Reset timer
        
		// print($"{gameObject} fired!");

		
    }
}