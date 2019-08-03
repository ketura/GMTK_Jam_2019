using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Unit))]
[RequireComponent(typeof(NavMeshAgent))]
public class MoveableUnit : MonoBehaviour
{
	public Waypoint Target;
	public bool MovementActive = true;

	private NavMeshAgent NavAgent;
	private Unit BaseUnit;

	// Start is called before the first frame update
	void Start()
	{
		NavAgent = GetComponent<NavMeshAgent>();
		BaseUnit = GetComponent<Unit>();

		if(Target != null)
		{
			SetNewDestination(Target);
		}

	}

	// Update is called once per frame
	void Update()
	{
		NavAgent.isStopped = !MovementActive;
	}


	public void SetNewDestination(Waypoint waypoint)
	{
		if(Target != null)
		{
			Target.RemoveTargetingUnit(BaseUnit);
		}

		Target = waypoint;

		if(waypoint == null)
		{
			NavAgent.isStopped = true;
		}
		else
		{
			if(!waypoint.TargetingUnits.Contains(BaseUnit))
			{
				waypoint.AddTargetingUnit(BaseUnit);
			}

			NavAgent.SetDestination(Target.transform.position);
			NavAgent.isStopped = false;
		}
	}

}
