using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBacking : MonoBehaviour
{
    [SerializeField] float radius;
    float distToNearest;
    GameObject target;
    [SerializeField] float distance;
    static string PlayerTag = "Blob";
    Vector3 targetpoint;
    public GameObject gizmo;
    bool enemyNearby;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
      
        Collider[] thingsInRadius = Physics.OverlapSphere(transform.position, radius);
        distToNearest = radius;
        target = null;
        enemyNearby = false;
        
        foreach (Collider c in thingsInRadius)
        {
            if (c.tag != PlayerTag)
            {
               
                continue;
            }
            Vector3 distance= transform.position - c.gameObject.transform.position;
            if (distToNearest >= distance.magnitude) {
                distToNearest = distance.magnitude;
                target = c.gameObject;
                enemyNearby = true;
            }

            

        }
        if (target != null) {

            Vector3 targetPos = target.transform.position;
            targetPos.y = transform.position.y;
            Vector3 targetpoint = transform.position - (targetPos - transform.position).normalized*distance;
           
        }
    }
}
