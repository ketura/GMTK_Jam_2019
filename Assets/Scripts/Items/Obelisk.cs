using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obelisk : MonoBehaviour
{
    public int maxHp;
    public float RecoveryTime;//how much time it takes for hp to increase
    public float range;
    float timer;
    public string PlayerTag = "Blob";
    //maybe add some kind of particles to indicate range
    void Start()
    {
        
    }

    void Update()
    {
        
            timer -= Time.deltaTime;
        if (timer < 0)
        {
            timer = RecoveryTime;

            Collider[] thingsInRadius = Physics.OverlapSphere(transform.position, range);


            foreach (Collider c in thingsInRadius)
            {
                if (c.tag != PlayerTag)
                {
                    continue;
                }

                if (maxHp > 0)
                {
                    c.gameObject.transform.parent.parent.SendMessage("Damage", -1);
                    maxHp--;
                }



            }
        }
        
    
    }
}
