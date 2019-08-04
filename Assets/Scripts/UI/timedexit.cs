using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class timedexit : MonoBehaviour
{
	// Start is called before the first frame update
	void Start()
	{
		ExitHandler.Instance.GracefulExit(20.0f);
	}

	// Update is called once per frame
	void Update()
	{
		
	}
}
