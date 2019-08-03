using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("s"))
        {
            SplitRandomBlob();
        }

        if (Input.GetKeyDown("m"))
        {
            CombineRandomBlobs();
        }
    }

    void SplitRandomBlob()
    {
        GameObject[] blobs = GameObject.FindGameObjectsWithTag("Blob");
        if (blobs.Length > 0)
        {
            blobs[Random.Range(0, blobs.Length)].GetComponent<Splitting>().Split();
        }
    }

    void CombineRandomBlobs()
    {
        GameObject[] blobs = GameObject.FindGameObjectsWithTag("Blob");
        if (blobs.Length < 2) return;
        int i = Random.Range(1, blobs.Length);
        int j = Random.Range(0, i);
        blobs[i].GetComponent<Splitting>().Combine(blobs[j]);
    }
}
