using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;
public class CommandController : Singleton<CommandController>
{
	private UnitController UController;

	public LayerMask ClickableMask;
	public LayerMask EnemyMask;
	public LayerMask GroundMask;

	public AudioClip[] SlimeNoises;

	// Start is called before the first frame update
	void Start()
	{
		UController = UnitController.Instance;
	}

	// Update is called once per frame
	void Update()
	{
		if(Input.GetButtonDown("Action") && !Input.GetButton("Select"))
		{
			Debug.Log("right clicking");
			if (UController.SelectionCount == 0)
				return;

			RaycastHit rayhit;

			//if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out rayhit, Mathf.Infinity, EnemyMask))
			//{
			//	Unit unit = rayhit.collider.GetComponentInParent<Unit>();
			//	if (unit == null)
			//		return;

			//	var selection = UController.GetSelectedUnits();

			//	IssueStopCommand(selection);

			//	Waypoint newTarget = UController.SpawnWaypoint(unit.transform.position);

			//	Unit unit = rayhit.collider.GetComponentInParent<Unit>();
			//	if (unit != null)
			//	{
			//		UController.ToggleSelection(unit);
			//	}
			//}


			if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out rayhit, Mathf.Infinity, GroundMask))
			{
				var selection = UController.GetSelectedUnits();

				IssueStopCommand(selection);

				Waypoint newTarget = UController.SpawnWaypoint(rayhit.point);

				newTarget.Initialize(selection);

				IssueMoveCommand(selection, newTarget);
					
				Unit unit = rayhit.collider.GetComponentInParent<Unit>();
				if (unit != null)
				{
					UController.ToggleSelection(unit);
				}

				int random = Random.Range(0, SlimeNoises.Length);
				AudioManager.Instance.PlayClip(SlimeNoises[random]);
			}

			
		}

		if(Input.GetButtonUp("Action"))
		{
			PlaySlimeSound();
		}
	}


	private void IssueStopCommand(Unit u)
	{
		MoveableUnit mu = u.GetComponent<MoveableUnit>();
		if (mu == null)
			return;

		mu.SetNewDestination(null);
	}

	private void IssueStopCommand(IEnumerable<Unit> units)
	{
		foreach (var u in units)
		{
			IssueStopCommand(u);
		}
	}

	private void IssueMoveCommand(Unit u, Waypoint target)
	{
		if (!target.Initialized)
		{
			target.Initialize(u);
		}

		MoveableUnit mu = u.GetComponent<MoveableUnit>();
		if (mu == null)
			return;

		mu.SetNewDestination(target);
	}

	private void IssueMoveCommand(IEnumerable<Unit> units, Waypoint target)
	{
		if(!target.Initialized)
		{
			target.Initialize(units);
		}
		
		foreach (var u in units)
		{
			IssueMoveCommand(u, target);
		}
	}

	public void PlaySlimeSound()
	{
		int random = Random.Range(0, SlimeNoises.Length);
		AudioManager.Instance.PlayClip(SlimeNoises[random]);
	}

}
