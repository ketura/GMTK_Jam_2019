using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SelectionType { None, Player, Turret, Enemy}

public class Unit : MonoBehaviour
{
	public bool Selectable;
	public SelectionType SelectionType;
	public string Name;

	private bool Selected;

	// Start is called before the first frame update
	void Start()
	{
		
	}

	// Update is called once per frame
	void Update()
	{
		
	}

	public void SetSelected(bool selected)
	{
		throw new System.NotImplementedException();
	}

	public Bounds GetBoundingBox()
	{
		throw new System.NotImplementedException();
	}
}
