using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TotalLifeTracker : MonoBehaviour
{

    public int HP => m_HP;
    public int StartingHP => m_StartingHP;

    private int m_HP, m_StartingHP;

    public string tag;
    // Start is called before the first frame update
    void Start()
    {
        Update();
    }

    // Update is called once per frame
    void Update()
    {
        m_HP = 0;
        m_StartingHP = 0;
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag(tag))
        {
            Life life = obj.GetComponent<Life>();
            if (life == null) continue;
            m_HP += life.HP;
            m_StartingHP = life.StartingHP;
        }
    }
}
