using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SplitController : MonoBehaviour
{
	UnitController UController;
    // Start is called before the first frame update
	void Start()
	{
		UController = UnitController.Instance;
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown("s"))
		{
			foreach(var unit in UController.GetSelectedUnits().ToList())
			{
				var blob = unit.GetComponent<Splitting>();
				if (blob == null)
					continue;

				blob.Split();
			}
		}

		else if (Input.GetKeyDown("m"))
		{
			var blobs = UController.GetSelectedUnits().Select(x => x.GetComponent<Splitting>()).Where(x => x != null);
			Splitting.Combine(blobs);
		}
	}

	//void SplitRandomBlob()
	//{
	//		GameObject[] blobs = GameObject.FindGameObjectsWithTag("Blob");
	//		if (blobs.Length > 0)
	//		{
	//				blobs[Random.Range(0, blobs.Length)].GetComponent<Splitting>().Split();
	//		}
	//}

	//void CombineRandomBlobs()
	//{
	//		GameObject[] blobs = GameObject.FindGameObjectsWithTag("Blob");
	//		if (blobs.Length < 2) return;
	//		int i = Random.Range(1, blobs.Length);
	//		int j = Random.Range(0, i);
	//		blobs[i].GetComponent<Splitting>().Combine(blobs[j]);
	//}
}
