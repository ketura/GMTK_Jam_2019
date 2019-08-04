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

	public float DefaultArrivalThreshold = 0.1f;
	public float MovementThreshold = 0.1f;
	public float ArrivalThreshold = 0.1f;
	public float ArrivalIncrement = 0.01f;

	public bool DebugOutput = false;

	private NavMeshAgent NavAgent;
	private Unit BaseUnit;

	private Vector3 LastFramePos;

	// Start is called before the first frame update
	void Awake()
	{
		NavAgent = GetComponent<NavMeshAgent>();
		BaseUnit = GetComponent<Unit>();

		if(Target != null)
		{
			SetNewDestination(Target);
		}

		LastFramePos = transform.position;
	}

	// Update is called once per frame
	void Update()
	{
		NavAgent.isStopped = !MovementActive;

		if (Target == null)
		{
			NavAgent.isStopped = true;
			NavAgent.stoppingDistance = ArrivalThreshold;
			return;
		}
			


		if (Vector3.Distance(transform.position, Target.transform.position) <= ArrivalThreshold)
		{
			SetNewDestination(null);
		}
		else if (DebugOutput)
		{
			Debug.Log(gameObject.name);
			Debug.Log($"\t{Vector3.Distance(transform.position, LastFramePos)}");
		}

		if(Vector3.Distance(transform.position, LastFramePos) <= MovementThreshold)
		{
			ArrivalThreshold += ArrivalIncrement;
			NavAgent.stoppingDistance = ArrivalThreshold;
		}

		LastFramePos = transform.position;
	}


	public void SetNewDestination(Waypoint waypoint)
	{
		if(Target != null)
		{
			Target.RemoveTargetingUnit(BaseUnit);
		}

		Target = waypoint;

		if(Target == null)
		{
			NavAgent.isStopped = true;
			NavAgent.stoppingDistance = ArrivalThreshold;
		}
		else
		{
			if(!Target.TargetingUnits.Contains(BaseUnit))
			{
				Target.AddTargetingUnit(BaseUnit);
			}
			ArrivalThreshold = DefaultArrivalThreshold;
			NavAgent.stoppingDistance = ArrivalThreshold;
			NavAgent.SetDestination(Target.transform.position);
			NavAgent.isStopped = false;

			
		}
	}

}
