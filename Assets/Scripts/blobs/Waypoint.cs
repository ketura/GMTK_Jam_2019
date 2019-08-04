using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
	public GameObject Post;

	public List<Unit> TargetingUnits
	{
		get;
		set;
	}


	public bool Initialized { get; private set; }
	// Start is called before the first frame update
	void Awake()
	{
		TargetingUnits = new List<Unit>();
	}

	// Update is called once per frame
	void Update()
	{
		if(Initialized)
		{
			TargetingUnits.RemoveAll(x => x == null);
			if(TargetingUnits.Count == 0)
			{
				Destroy(this.gameObject);
			}
		}
	}

	public void Initialize(IEnumerable<Unit> units)
	{
		if (Initialized)
			return;

		TargetingUnits.AddRange(units);

		Initialized = true;
	}

	public void Initialize(Unit unit)
	{
		if (Initialized)
			return;

		Initialize(new List<Unit>() { unit });
	}

	public void AddTargetingUnit(Unit unit)
	{
		if (TargetingUnits.Contains(unit))
			return;

		TargetingUnits.Add(unit);
	}

	public void RemoveTargetingUnit(Unit unit)
	{
		if (!TargetingUnits.Contains(unit))
			return;

		TargetingUnits.Remove(unit);
	}
}
