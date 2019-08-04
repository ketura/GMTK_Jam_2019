using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Unit))]
[RequireComponent(typeof(NavMeshAgent))]
public class MoveableUnit : MonoBehaviour
{
	public Waypoint Waypoint;

	public bool MovementActive = true;

	public float DefaultArrivalThreshold = 0.1f;
	public float MovementThreshold = 0.01f;
	public float ArrivalThreshold = 0.1f;
	public float ArrivalIncrement = 0.05f;

	public bool DebugOutput = false;

	private NavMeshAgent NavAgent;
	private Unit BaseUnit;

	private Vector3 LastFramePos;

	//public audio

	// Start is called before the first frame update
	void Awake()
	{
		NavAgent = GetComponent<NavMeshAgent>();
		BaseUnit = GetComponent<Unit>();

		if(Waypoint != null)
		{
			SetNewDestination(Waypoint);
		}

		LastFramePos = transform.position;
	}

	// Update is called once per frame
	void Update()
	{
		NavAgent.isStopped = !MovementActive;

		if (Waypoint == null)
		{
			NavAgent.isStopped = true;
			NavAgent.stoppingDistance = ArrivalThreshold;
			return;
		}
			


		if (Vector3.Distance(transform.position, Waypoint.transform.position) <= ArrivalThreshold)
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
		if(Waypoint != null)
		{
			Waypoint.RemoveTargetingUnit(BaseUnit);
		}

		Waypoint = waypoint;

		if(Waypoint == null)
		{
			NavAgent.isStopped = true;
			NavAgent.stoppingDistance = ArrivalThreshold;
		}
		else
		{
			if(!Waypoint.TargetingUnits.Contains(BaseUnit))
			{
				Waypoint.AddTargetingUnit(BaseUnit);
			}
			ArrivalThreshold = DefaultArrivalThreshold;
			NavAgent.stoppingDistance = ArrivalThreshold;
			NavAgent.SetDestination(Waypoint.transform.position);
			NavAgent.isStopped = false;

			
		}
	}
}
