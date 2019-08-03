using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Utilities;


[RequireComponent(typeof(LineRenderer))]
public class SelectionMouseHandler : MonoBehaviour, IMouseHandler
{
	public GameObject DebugMarker;
	public LineRenderer SelectionBoxGUI;

	public GameObject GreenSelect;
	public GameObject RedSelect;

	public List<Unit> Selection;
	public List<Unit> ToggleSelection;

	private Vector2 SelectStart;
	private Vector2 SelectEnd;
	private bool CurrentlySelecting = false;
	private SelectionMode CurrentMode = SelectionMode.Standard;

	// Use this for initialization
	void Start()
	{
		SelectionBoxGUI = GetComponent<LineRenderer>();
		Selection = new List<Unit>();


		SelectionController.Instance.SelectionMouseHandler = this;
	}

	// Update is called once per frame
	void Update()
	{
		if (CurrentlySelecting)
		{
			SelectionBoxGUI.enabled = true;

			float depth = -8f;

			//To actually be able to use the default linerenderer, you need twice as many points
			// as one might assume.  So to make a 4-point rectangle, we need 8 points, and the 
			// corners all need to be "twisted" to make the whole thing line up. Think of making
			// the whole thing out of a long twist-tie; you can't just bend the corners if you 
			// want it to line up straight.

			float xoffset;
			if (SelectEnd.x > SelectStart.x)
				xoffset = -0.01f;
			else
				xoffset = 0.01f;

			SelectionBoxGUI.SetPosition(0, new Vector3(SelectStart.x, SelectStart.y, depth));
			SelectionBoxGUI.SetPosition(1, new Vector3(SelectEnd.x + xoffset, SelectStart.y, depth));

			SelectionBoxGUI.SetPosition(2, new Vector3(SelectEnd.x, SelectStart.y, depth));
			if (SelectEnd.y < SelectStart.y)
				SelectionBoxGUI.SetPosition(3, new Vector3(SelectEnd.x, SelectEnd.y + 0.01f, depth));
			else
				SelectionBoxGUI.SetPosition(3, new Vector3(SelectEnd.x, SelectEnd.y - 0.01f, depth));

			SelectionBoxGUI.SetPosition(4, new Vector3(SelectEnd.x, SelectEnd.y, depth));
			SelectionBoxGUI.SetPosition(5, new Vector3(SelectStart.x - xoffset, SelectEnd.y, depth));

			SelectionBoxGUI.SetPosition(6, new Vector3(SelectStart.x, SelectEnd.y, depth));
			SelectionBoxGUI.SetPosition(7, new Vector3(SelectStart.x, SelectStart.y, depth));
		}
		else
			SelectionBoxGUI.enabled = false;
	}

	public List<Unit> GetSelection()
	{
		List<Unit> newlist = new List<Unit>();
		foreach (Unit unit in Selection)
			if (unit != null)
				newlist.Add(unit);
		//Selection.Clear();
		return newlist;
	}

	public void LeftMove(Vector2 target)
	{
		//not used
	}

	public void LeftDown(Vector2 target, int info)
	{
		//StartSelection
		target = Camera.main.ScreenToWorldPoint(target);
		CurrentlySelecting = true;
		CurrentMode = (SelectionMode)info;
		SelectStart = target;
		SelectEnd = target;
		if (CurrentMode == SelectionMode.Standard)
			ClearSelection();
		else if (CurrentMode == SelectionMode.Toggle)
			ToggleSelection = new List<Unit>();
	}

	public void LeftDrag(Vector2 target)
	{
		//MoveSelection
		target = Camera.main.ScreenToWorldPoint(target);
		if (CurrentlySelecting)
		{
			//Debug.Log("Select Drag");
			SelectEnd = target;
			SelectUnitsInBounds();
		}
	}

	public void LeftUp(Vector2 target)
	{
		//ReleaseSelection
		target = Camera.main.ScreenToWorldPoint(target);
		if (CurrentlySelecting)
		{
			CurrentlySelecting = false;
			SelectEnd = target;
			SelectUnitsInBounds();
			if (CurrentMode == SelectionMode.Toggle)
			{
				foreach (Unit unit in ToggleSelection)
				{
					if (Selection.Contains(unit))
						RemoveUnit(unit);
					else
						SelectUnit(unit);
				}

				foreach (Unit unit in Selection)
					unit.SetSelected(true);
			}

			//Debug.Log(Selection.Count + " selected.");
		}
	}

	public void RightClick(Vector2 point)
	{
		//ActionButton
		//right-clicked while selecting cancels the box
		if (CurrentlySelecting)
		{
			CurrentlySelecting = false;
			ClearSelection();
		}
		else if (Selection.Count > 0)
		{
			point = Camera.main.ScreenToWorldPoint(point);
			RaycastHit2D hit = Physics2D.Raycast(point, -Vector2.up, Mathf.Infinity);
			Unit target = null;

			if (hit.collider != null)
			{
				target = hit.collider.gameObject.GetComponentInChildren<Unit>();
				if (target != null && (target.Selectable == false || target.SelectionType != SelectionType.Enemy || target.SelectionType != SelectionType.Turret))
				{
					target = null;
				}
			}

			//foreach (Unit p in Selection)
			//{
			//	if (target != null)
			//		p.ChangeTarget(target);
			//	else
			//		p.ChangeTarget(new Vector3(point.x, point.y, 1.0f));
			//}
		}
	}

	public void SelectAll()
	{
		ClearSelection();
		//foreach (Tile unit in GridMap.GetAllGroundTiles())
			//SelectUnit(unit);

	}

	public void SelectUnit(Unit unit)
	{
		if (!Selection.Contains(unit))
			Selection.Add(unit);
		unit.SetSelected(true);
	}

	public void RemoveUnit(Unit unit)
	{
		if (Selection.Contains(unit))
			Selection.Remove(unit);
		unit.SetSelected(false);
	}

	private void AddToggleUnit(Unit unit)
	{
		if (!ToggleSelection.Contains(unit))
			ToggleSelection.Add(unit);
	}

	private void RemoveToggleUnit(Unit unit)
	{
		if (ToggleSelection.Contains(unit))
			ToggleSelection.Remove(unit);
	}



	public void ClearSelection()
	{
		//Debug.Log("Clearing selection");
		foreach (Unit mc in Selection)
			mc.SetSelected(false);
		Selection.Clear();
	}

	private void ClearToggleSelection()
	{
		ToggleSelection.Clear();
	}

	private Unit SelectSingleUnit(Vector2 target)
	{
		RaycastHit2D hit = Physics2D.Raycast(target, Vector2.zero, Mathf.Infinity);

		if (hit.collider != null)
		{
			var unit = hit.collider.GetComponent<Unit>();
			if (unit != null && unit.Selectable && unit.SelectionType == SelectionType.Player)
				return unit;
		}

		return null;
	}

	private void SelectUnitsInBounds()
	{
		if (CurrentMode == SelectionMode.Standard)
			ClearSelection();
		else if (CurrentMode == SelectionMode.Toggle)
			ClearToggleSelection();

		IEnumerable<Unit> Units = UnitController.Instance.GetActiveUnits();

		//Debug.Log(Units.Count);

		Bounds selection = new UnityEngine.Bounds();
		selection.SetMinMax(Vector3.Min(SelectStart, SelectEnd), Vector3.Max(SelectStart, SelectEnd));

		if (selection.extents == Vector3.zero)
		{
			Unit unit = SelectSingleUnit(selection.center);
			if (unit != null)
			{
				switch (CurrentMode)
				{
					case SelectionMode.Remove:
						RemoveUnit(unit);
						break;

					case SelectionMode.Toggle:
						AddToggleUnit(unit);
						break;

					default:
					case SelectionMode.Add:
					case SelectionMode.Standard:
						SelectUnit(unit);
						break;
				}
			}
		}

		foreach (Unit unit in Units)
		{
			//Debug
			if (!unit.Selectable || unit.SelectionType != SelectionType.Player)
				continue;

			if (unit.GetBoundingBox().Intersects(selection))
			{
				switch (CurrentMode)
				{
					case SelectionMode.Remove:
						RemoveUnit(unit);
						break;

					case SelectionMode.Toggle:
						AddToggleUnit(unit);
						break;

					default:
					case SelectionMode.Add:
					case SelectionMode.Standard:
						SelectUnit(unit);
						break;
				}
			}
		}

		if (CurrentMode == SelectionMode.Toggle)
		{
			foreach (Unit unit in Selection)
				unit.SetSelected(true);

			foreach (Unit unit in ToggleSelection)
			{
				if (Selection.Contains(unit))
					unit.SetSelected(false);
				else
					unit.SetSelected(true);
			}
		}

		Debug.Log(Selection.Count);
	}

}
