using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropBlobOnDeath : MonoBehaviour
{
    public GameObject BlobPrefab;
    public float chance = 0.3f;
    public int SpawnHP = 1;

    void OnDestroy()
    {
        if (Random.Range(0,1) < chance)
        {
            GameObject blob = Instantiate(BlobPrefab, gameObject.transform.position, gameObject.transform.rotation);
            blob.GetComponent<Life>().HP = SpawnHP;
            blob.GetComponent<Life>().StartingHP = SpawnHP;
        }
    }
}