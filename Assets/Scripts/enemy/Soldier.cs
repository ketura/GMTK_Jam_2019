using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MoveableUnit))]
[RequireComponent(typeof(Life))]
[RequireComponent(typeof(EnemyBacking))]
[RequireComponent(typeof(FacePlayer))]
[RequireComponent(typeof(Unit))]
[RequireComponent(typeof(Weapon))]
public class Soldier : MonoBehaviour
{
	private UnitController UController;
	private Waypoint MoveTarget;
	// Start is called before the first frame update
	void Start()
	{
		UController = UnitController.Instance;
		MoveTarget = UController.SpawnWaypoint(transform.position, false);
		GetComponent<EnemyBacking>().BackingTarget = MoveTarget;
	}

	// Update is called once per frame
	void Update()
	{
		
	}
}
