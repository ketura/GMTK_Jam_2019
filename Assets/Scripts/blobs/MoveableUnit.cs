using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Unit))]
[RequireComponent(typeof(NavMeshAgent))]
public class MoveableUnit : MonoBehaviour
{
	public Transform Target;
	public bool MovementActive = true;

	private NavMeshAgent NavAgent;

	// Start is called before the first frame update
	void Start()
	{
		NavAgent = GetComponent<NavMeshAgent>();

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


	public void SetNewDestination(Transform transform)
	{
		Target = transform;
		NavAgent.SetDestination(Target.position);
	}

}
