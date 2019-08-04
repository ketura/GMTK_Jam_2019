using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

using Utilities;
using RektTransform;


public enum SelectionMode { Standard, Add, Remove, Toggle };

public class SelectionController : Singleton<SelectionController>
{
	public IMouseHandler SelectionMouseHandler;

	public LayerMask ClickableMask;

	public RectTransform SelectionBox;

	private Vector2 StartPos;
	private Vector2 EndPos;

	private bool LeftDown = false;
	private bool RightDown = false;
	private bool MidDown = false;

	private UnitController UController;

	private Camera UICamera;

	private List<Unit> TempSelection;

	// Start is called before the first frame update
	void Start()
	{
		UController = UnitController.Instance;
		SelectionBox.gameObject.SetActive(false);
		UICamera = GameObject.FindGameObjectWithTag("UICamera").GetComponent<Camera>();
		TempSelection = new List<Unit>();
	}

	// Update is called once per frame
	void Update()
	{
		bool ctrl = Input.GetButton("Toggle");
		bool shift = Input.GetButton("AddGroup");
		bool alt = Input.GetButton("RemoveGroup");

		if (Input.GetButtonDown("Select"))
		{
			Debug.Log("Clicking");
			RaycastHit rayhit;

			StartPos = UICamera.ScreenToViewportPoint(Input.mousePosition);
			TempSelection = new List<Unit>();

			SelectionBox.offsetMin = Vector2.zero;
			SelectionBox.offsetMax = Vector2.zero;
			SelectionBox.gameObject.SetActive(true);
			//StartPos = Input.mousePosition;
			//Debug.Log(StartPos);

			if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out rayhit, Mathf.Infinity, ClickableMask))
			{
				Unit unit = rayhit.collider.GetComponentInParent<Unit>();
				if(unit == null)
				{
					if(!ctrl && !shift && !alt)
					{
						UController.ClearSelection();
					}
					return;
				}

				if(ctrl)
				{
					UController.ToggleSelection(unit);
				}
				else if(alt)
				{
					UController.RemoveUnitFromSelection(unit);
				}
				else if(shift)
				{
					UController.AddUnitToSelection(unit);
				}
				else
				{
					UController.ClearSelection();
					UController.AddUnitToSelection(unit);
				}
				
				
			}
			else
			{
				if (!ctrl && !shift && !alt)
				{
					UController.ClearSelection();
				}
				return;
			}

			

		}
		else if(Input.GetButton("Select"))
		{
			if(Input.GetButton("Action"))
			{
				SelectionBox.gameObject.SetActive(false);
			}

			

			EndPos = UICamera.ScreenToViewportPoint(Input.mousePosition);
			//EndPos = Input.mousePosition;
			Debug.Log(EndPos);

			var canvas = SelectionBox.GetRootCanvas().GetComponent<RectTransform>();

			float width = canvas.GetWidth();//Screen.width;
			float height = canvas.GetHeight();// Screen.height;

			float x1 = StartPos.x * width;
			float x2 = EndPos.x * width;
			float y1 = StartPos.y * height;
			float y2 = EndPos.y * height;

			

			SelectionBox.anchoredPosition = Vector2.zero;

			SelectionBox.offsetMin = new Vector2(Mathf.Min(x1, x2), Mathf.Min(y1, y2));
			SelectionBox.offsetMax = new Vector2(Mathf.Max(x1, x2), Mathf.Max(y1, y2));

			if (ctrl)
			{
				UController.ToggleSelection(TempSelection);
			}
			else if (alt)
			{
				UController.AddUnitsToSelection(TempSelection);
			}
			else //if (shift)
			{
				UController.RemoveUnitsFromSelection(TempSelection);
			}

			TempSelection = new List<Unit>();

			//If we cancelled using right click, this will have us exiting at a convenient time.
			if (!SelectionBox.gameObject.activeSelf)
			{
				return;
			}


			Rect selectRect = new Rect(StartPos.x, StartPos.y, EndPos.x - StartPos.x, EndPos.y - StartPos.y);

			var currentlySelected = (List<Unit>)UController.GetSelectedUnits();

			foreach(var unit in UController.GetActiveUnits())
			{
				if(selectRect.Contains(Camera.main.WorldToViewportPoint(unit.transform.position), true))
				{
					if(ctrl)
					{
						TempSelection.Add(unit);
					}
					else if(alt)
					{
						if (currentlySelected.Contains(unit))
						{
							TempSelection.Add(unit);
						}
					}
					else //if(shift)
					{
						if (!currentlySelected.Contains(unit))
						{
							TempSelection.Add(unit);
						}
					}
					
				}
			}

			if (ctrl)
			{
				UController.ToggleSelection(TempSelection);
			}
			else if (alt)
			{
				UController.RemoveUnitsFromSelection(TempSelection);
			}
			else //if (shift)
			{
				UController.AddUnitsToSelection(TempSelection);
			}

		}
		else if(Input.GetButtonUp("Select"))
		{
			SelectionBox.gameObject.SetActive(false);

		}




		return;
		bool Filter = EventSystem.current.IsPointerOverGameObject();
		//need a more robust priority system.

		Rect screenRect = new Rect(0, 0, Screen.width, Screen.height);
		if (!screenRect.Contains(Input.mousePosition))
			return;

		//if (!Filter)
		//{
		//	if (Input.GetAxisRaw("Mouse ScrollWheel") != 0)
		//		cam.Zoom(Input.GetAxisRaw("Mouse ScrollWheel"));
		//}

		if (!Filter || MidDown)
		{
			if (Input.GetButton("Pan"))
			{
				//cam.Pan(new Vector2(-Input.GetAxisRaw("Mouse X"), -Input.GetAxisRaw("Mouse Y")), Input.mousePosition);
				MidDown = true;
			}
			else
				MidDown = false;
		}


		//Debug.Log("selecting");

		if (!Filter)
		{
			//Debug.Log("Mouse");
			SelectionMode mode = SelectionMode.Standard;
			if (Input.GetButton("Toggle"))
				mode = SelectionMode.Toggle;
			else if (Input.GetButton("AddGroup"))
				mode = SelectionMode.Add;
			else if (Input.GetButton("RemoveGroup"))
				mode = SelectionMode.Remove;

			if (Input.GetButton("Select"))
			{
				if (!LeftDown)
				{
					SelectionMouseHandler.LeftDown(Input.mousePosition, (int)mode);
					LeftDown = true;
				}
				else
					SelectionMouseHandler.LeftDrag(Input.mousePosition);
			}
			else
			{
				SelectionMouseHandler.LeftUp(Input.mousePosition);
				LeftDown = false;
			}

			if (Input.GetButton("Action"))
			{
				if (!RightDown)
				{
					SelectionMouseHandler.RightClick(Input.mousePosition);
					RightDown = true;
				}
			}
			else
				RightDown = false;

		}
		else
		{
			if (LeftDown)
			{
				if (Input.GetButton("Select"))
					SelectionMouseHandler.LeftDrag(Input.mousePosition);
				else
					SelectionMouseHandler.LeftUp(Input.mousePosition);
			}
			else if (Input.GetButton("Select"))
				LeftDown = true;
		}

	}
}
