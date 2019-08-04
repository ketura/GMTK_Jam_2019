using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacePlayer : MonoBehaviour
{
    public float SearchRadius;
    public float RotationSpeed;
	  public string PlayerTag = "Blob";
	public GameObject TurnObject;

	  float distToNearest;
    GameObject target;
    
    [SerializeField] GameObject defaultTarget;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Collider[] thingsInRadius = Physics.OverlapSphere(transform.position, SearchRadius);
        distToNearest = SearchRadius;
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
            targetPos.y = TurnObject.transform.position.y;
            Vector3 targetDir = targetPos - TurnObject.transform.position;

            Vector3 newDir = Vector3.RotateTowards(TurnObject.transform.forward, targetDir, RotationSpeed, 0.0f);

			TurnObject.transform.rotation = Quaternion.LookRotation(newDir);
        }

    }

}
