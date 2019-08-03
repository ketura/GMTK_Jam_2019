using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Utilities;

public class UnitController : Singleton<UnitController>
{
	public GameObject WaypointPrefab;

	private List<Unit> ActiveUnits { get; set; }

	private List<Unit> SelectedUnits { get; set; }

	private List<Transform> Waypoints { get; set; }

	public int SelectionCount => SelectedUnits.Count;

	// Start is called before the first frame update
	void Start()
	{
		ActiveUnits = new List<Unit>();
		SelectedUnits = new List<Unit>();
		Waypoints = new List<Transform>();
	}

	// Update is called once per frame
	void Update()
	{
		
	}

	public IEnumerable<Unit> GetActiveUnits()
	{
		return ActiveUnits;
	}

	public IEnumerable<Unit> GetSelectedUnits()
	{
		return SelectedUnits;
	}

	public void AddUnit(Unit u)
	{
		if (ActiveUnits.Contains(u))
			return;

		ActiveUnits.Add(u);
	}

	public void RemoveUnit(Unit u)
	{
		if (!ActiveUnits.Contains(u))
			return;

		ActiveUnits.Remove(u);
	}

	public void AddUnits(IEnumerable<Unit> units)
	{
		foreach (var unit in units)
		{
			AddUnit(unit);
		}
	}

	public void RemoveUnits(IEnumerable<Unit> units)
	{
		foreach (var unit in units)
		{
			RemoveUnit(unit);
		}
	}



	public void AddUnitToSelection(Unit u)
	{
		if (SelectedUnits.Contains(u))
			return;

		u.Select();
		SelectedUnits.Add(u);
	}

	public void RemoveUnitFromSelection(Unit u)
	{
		if (!SelectedUnits.Contains(u))
			return;

		u.Deselect();
		SelectedUnits.Remove(u);
	}

	public void AddUnitsToSelection(IEnumerable<Unit> units)
	{
		foreach (var unit in units)
		{
			AddUnitToSelection(unit);
		}
	}

	public void RemoveUnitsFromSelection(IEnumerable<Unit> units)
	{
		foreach (var unit in units)
		{
			RemoveUnitFromSelection(unit);
		}
	}

	public void ClearSelection()
	{
		RemoveUnitsFromSelection(SelectedUnits.ToArray());
	}

	public void UpdateSelection(Unit u, bool selected)
	{
		u.SetSelected(selected);
		if(selected)
		{
			AddUnitToSelection(u);
		}
		else
		{
			RemoveUnitFromSelection(u);
		}
	}

	public void ToggleSelection(Unit u)
	{
		UpdateSelection(u, !u.Selected);
	}

	public Waypoint SpawnWaypoint(Vector3 pos)
	{
		var go = Instantiate(WaypointPrefab, pos, Quaternion.identity);
		return go.GetComponent<Waypoint>();
	}
}
