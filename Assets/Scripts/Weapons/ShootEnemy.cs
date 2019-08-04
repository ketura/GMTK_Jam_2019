using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Weapon))]
public class ShootEnemy : MonoBehaviour 
{
    public float SearchRadius;
    public string Tag;

    float distToNearest;
    GameObject target;
    
    [SerializeField] GameObject defaultTarget;
    public bool AutoAttack = true;
    private Weapon Weapon;
    // Start is called before the first frame update
    void Start()
    {
        if (Weapon == null) Weapon = GetComponent<Weapon>();
    }

    // Update is called once per frame
    void Update()
    {
        if (AutoAttack && Weapon.CanFire)
        {
            Collider[] thingsInRadius = Physics.OverlapSphere(transform.position, SearchRadius);
            distToNearest = SearchRadius;
            target = defaultTarget;
            foreach (Collider c in thingsInRadius)
            {
                if (c.tag != Tag)
                {
                        continue;
                }

                Vector3 distance= transform.position - c.gameObject.transform.position;
                if (distToNearest >= distance.magnitude) {
                    distToNearest = distance.magnitude;
                    target = c.gameObject;
                    
                }

            }
            // print($"{gameObject} shooting at {target}");
            if (target != null)
            {
                Weapon.Fire(target);
            }
        }
        // else print("Not firing: autoattack disabled or weapon can't fire");

    }
}