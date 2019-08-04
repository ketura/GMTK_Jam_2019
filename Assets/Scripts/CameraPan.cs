using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPan : MonoBehaviour
{
	public Vector2 PanFactors = new Vector2(1.0f, 1.0f);
	public Vector2 PanJump = new Vector2(1.0f, 1.0f);
	public float MinZoom = 30.0f;
	public float MaxZoom = 80.0f;
	public float ZoomStep = 10.0f;
	public float CurrentZoom;
	public float MinX = -100;
	public float MaxX = 100;
	public float MinZ = -100;
	public float MaxZ = 100;

	public float HoverLevel = 10;
	public float HoverAcceleration = 1.0f;
	public Vector3 Orientation = new Vector3(30.0f, 20.0f, 0.0f);
	public string GroundTag = "Ground";
	public LayerMask GroundMask;

	public bool OnRails = false;
	public Transform LookatTarget = null;

	private Camera AttachedCamera;

	// Start is called before the first frame update
	void Start()
	{
		CurrentZoom = (MaxZoom + MinZoom) / 2;

		AttachedCamera = GetComponent<Camera>();
	}

	// Update is called once per frame
	void Update()
	{

		if(Input.GetButton("Pan"))
		{
			Cursor.visible = false;
			Cursor.lockState = CursorLockMode.Locked;

			float dx = Input.GetAxis("Mouse X") * PanFactors.x;
			float dy = Input.GetAxis("Mouse Y") * PanFactors.y;

			this.transform.Translate(-dx, 0, -dy, Space.World);
		}
		else
		{
			float dx = Input.GetAxis("Horizontal") * PanJump.x;
			float dy = Input.GetAxis("Vertical") * PanJump.y;

			this.transform.Translate(dx, 0, dy, Space.World);
		}

		if(Input.GetButtonUp("Pan"))
		{
			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;
		}

		if (Input.GetAxis("Mouse ScrollWheel") > 0)
		{
			CurrentZoom = Mathf.Clamp(CurrentZoom - ZoomStep, MinZoom, MaxZoom);
		}

		if (Input.GetAxis("Mouse ScrollWheel") < 0)
		{
			CurrentZoom = Mathf.Clamp(CurrentZoom + ZoomStep, MinZoom, MaxZoom);
		}

		AttachedCamera.fieldOfView = CurrentZoom;

		RaycastHit rayhit;
		if (Physics.Raycast(this.transform.position, Vector3.down, out rayhit, Mathf.Infinity, GroundMask))
		{
			float distance = Vector3.Distance(this.transform.position, rayhit.point);
			if (distance > (HoverLevel * 1.05))
			{
				this.transform.Translate(0, -HoverAcceleration * Time.deltaTime, 0, Space.World);
			}

			if (distance < (HoverLevel * 0.95))
			{
				this.transform.Translate(0, HoverAcceleration * Time.deltaTime, 0, Space.World);
			}

		}

		if (AttachedCamera.transform.position.x < MinX)
		{
			AttachedCamera.transform.position = new Vector3(MinX, AttachedCamera.transform.position.y, AttachedCamera.transform.position.z);
		}

		if (AttachedCamera.transform.position.x > MaxX)
		{
			AttachedCamera.transform.position = new Vector3(MaxX, AttachedCamera.transform.position.y, AttachedCamera.transform.position.z);
		}

		if (AttachedCamera.transform.position.z < MinZ)
		{
			AttachedCamera.transform.position = new Vector3(AttachedCamera.transform.position.x, AttachedCamera.transform.position.y, MinZ);
		}

		if (AttachedCamera.transform.position.z > MaxZ)
		{
			AttachedCamera.transform.position = new Vector3(AttachedCamera.transform.position.x, AttachedCamera.transform.position.y, MaxZ);
		}
	}
}
