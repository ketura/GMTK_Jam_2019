using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(MoveableUnit))]
public class EnemyBacking : MonoBehaviour
{
    public float SearchRadius;
	  public float BackingDistance;
	  public string PlayerTag = "Blob";

	  float distToNearest;
    GameObject target;
    
    Vector3 targetpoint;
    public Waypoint BackingTarget;
    public bool EnemyNearby { get; private set; }

	private MoveableUnit MovingUnit;
    
    // Start is called before the first frame update
    void Start()
    {
		MovingUnit = GetComponent<MoveableUnit>();
    }

    // Update is called once per frame
    void Update()
    {
      
        Collider[] thingsInRadius = Physics.OverlapSphere(transform.position, SearchRadius);
        distToNearest = SearchRadius;
        target = null;
        EnemyNearby = false;
        
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
				EnemyNearby = true;
            }

            

        }
        if (target != null) {

            Vector3 targetPos = target.transform.position;
            targetPos.y = transform.position.y;
            Vector3 targetpoint = transform.position - (targetPos - transform.position).normalized * BackingDistance;
					  BackingTarget.transform.position = targetpoint;

			MovingUnit.SetNewDestination(BackingTarget);

				}
    }
}
