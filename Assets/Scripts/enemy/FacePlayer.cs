using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacePlayer : MonoBehaviour
{
    [SerializeField] float radius;
    [SerializeField] float speed;
    float distToNearest;
    GameObject target;
    static string PlayerTag ="Blob";
    [SerializeField] GameObject defaultTarget;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Collider[] thingsInRadius = Physics.OverlapSphere(transform.position, radius);
        distToNearest = radius;
        target = defaultTarget;
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
                
            }

           

        }
        if (target != null)
        {
            Vector3 targetPos = target.transform.position;
            targetPos.y = transform.position.y;
            Vector3 targetDir = targetPos - transform.position;

            Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, speed, 0.0f);

            transform.rotation = Quaternion.LookRotation(newDir);
        }

    }

}
