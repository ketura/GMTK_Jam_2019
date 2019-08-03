using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SelectionType { None, Player, Turret, Enemy}

public class Unit : MonoBehaviour
{
	public bool Selectable;
	public SelectionType SelectionType;
	public string Name;

	public GameObject SelectionCircle;
	public Transform SelectionAnchor;


	private UnitController UController;
	public bool Selected { get; private set; }

	// Start is called before the first frame update
	void Start()
	{
		UController = UnitController.Instance;
		UController.AddUnit(this);
	}

	// Update is called once per frame
	void Update()
	{
		//if (Input.GetKeyDown("s"))
		//{
		//	Select();
		//}
		//if (Input.GetKeyDown("d"))
		//{
		//	Deselect();
		//}
	}

	public void Select()
	{
		if (!Selected)
		{
			SetSelected(true);
		}
	}

	public void Deselect()
	{
		//if (Selected)
		//{
			SetSelected(false);
		//}
	}

	public void ToggleSelection()
	{
		SetSelected(!Selected);
	}

	public void SetSelected(bool selected)
	{
		//no point shuffling bits if we're not actually changing state
		if (selected == Selected)
			return;

		//avoid changing anything if disabled etc
		if (!Selectable)
			return;

		Selected = selected;

		if(selected)
		{
			SelectionCircle.SetActive(true);
		}
		else
		{
			SelectionCircle.SetActive(false);
		}
	}

	public Bounds GetBoundingBox()
	{
		throw new System.NotImplementedException();
	}
}
