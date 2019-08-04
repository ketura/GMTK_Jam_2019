using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class HPTracker : MonoBehaviour
{

    public Life PoolToTrack;
    private RectTransform RT;
    // Start is called before the first frame update
    void Start()
    {
        RT = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        float ratio = (float) PoolToTrack.HP / PoolToTrack.StartingHP;
        RT.anchorMax = new Vector2(ratio, RT.anchorMax.y);
    }
}
