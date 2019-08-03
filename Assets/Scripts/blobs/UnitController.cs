using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Utilities;

public class UnitController : Singleton<UnitController>
{
	private List<Unit> ActiveUnits { get; set; }

	// Start is called before the first frame update
	void Start()
	{
		ActiveUnits = new List<Unit>();
	}

	// Update is called once per frame
	void Update()
	{
		
	}

	public IEnumerable<Unit> GetActiveUnits()
	{
		return ActiveUnits;
	}
}
