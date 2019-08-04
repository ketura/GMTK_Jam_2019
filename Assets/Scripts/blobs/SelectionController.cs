using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

using Utilities;
//using RektTransform;


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

	// Start is called before the first frame update
	void Start()
	{
		UController = UnitController.Instance;
		SelectionBox.gameObject.SetActive(false);
		UICamera = GameObject.FindGameObjectWithTag("UICamera").GetComponent<Camera>();
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

			//StartPos = UICamera.ScreenToViewportPoint(Input.mousePosition);
			StartPos = Input.mousePosition;
			Debug.Log(StartPos);

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

		}
		else if(Input.GetButton("Select"))
		{
			EndPos = UICamera.ScreenToViewportPoint(Input.mousePosition);
			EndPos = Input.mousePosition;
			Debug.Log(EndPos);

			//var canvas = SelectionBox.GetRootCanvas().GetComponent<RectTransform>();

			float width = Screen.width;
			float height = Screen.height;

			float x1 = StartPos.x;// * width;
			float x2 = EndPos.x;// * width;
			float y1 = StartPos.y;// * height;
			float y2 = EndPos.y;// * height;

			SelectionBox.gameObject.SetActive(true);

			SelectionBox.sizeDelta = new Vector2(Mathf.Abs(x1 - x2), Mathf.Abs(y1 - y2));
			//SelectionBox.

			//SelectionBox.Left(Mathf.Min(x1, x2));
			//SelectionBox.Right(Mathf.Max(x1, x2));
			//SelectionBox.Bottom(Mathf.Min(y1, y2));
			//SelectionBox.Top(Mathf.Max(y1, y2));

			
		}
		else
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
